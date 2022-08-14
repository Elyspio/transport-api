using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Spectre.Console;
using Transport.Api.Abstractions.Common.Helpers;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Transports.FuelStation;
using Transport.Api.Adapters;

namespace Transport.Api.Db.Cache.Services
{
	internal class DbImportService
	{
		private const string MergePath = "P:\\own\\mobile\\transport-api\\back\\Tools\\Db.Cache\\merged.json";
		private const string FileDataPath = "P:\\own\\mobile\\transport-api\\back\\Tools\\Db.Cache\\cache.json";
		private readonly ILogger<DbImportService> logger;
		private readonly IPriceRepository priceRepository;
		private readonly IFuelStationRepository fuelStationRepository;
		private readonly PublicFilesClient publicFiles;

		public DbImportService(IPriceRepository priceRepository, IFuelStationRepository fuelStationRepository, PublicFilesClient publicFiles, ILogger<DbImportService> logger)
		{
			this.logger = logger;
			this.priceRepository = priceRepository;
			this.fuelStationRepository = fuelStationRepository;
			this.publicFiles = publicFiles;
		}


		private async Task<List<FuelStationData>> GetMergedData()
		{
			if (File.Exists(MergePath))
			{
				using var file = File.OpenRead(MergePath);
				using var sr = new StreamReader(file);
				using JsonReader reader = new JsonTextReader(sr);
				var serializer = new JsonSerializer() { Converters = { new StringEnumConverter() } };
				return serializer.Deserialize<List<FuelStationData>>(reader)!;
			}
			else
			{
				var file = GetFileData();

				var content = await publicFiles.GetFileContentAsync(file.Id);

				using var sr = new StreamReader(content.Stream);

				using JsonReader reader = new JsonTextReader(sr);

				var serializer = new JsonSerializer() { Converters = { new StringEnumConverter() } };

				return serializer.Deserialize<List<FuelStationData>>(reader)!;
			}
		}


		public async Task Import()
		{

			await AnsiConsole.Progress()
			.AutoClear(false)
			.Columns(new TaskDescriptionColumn { Alignment = Justify.Left }, new ElapsedTimeColumn(), new SpinnerColumn())
			.StartAsync(async ctx =>
			{
				var dlTask = ctx.AddTask("Fetch data");
				var data = await GetMergedData();
				dlTask.Description = "Fetched data";
				dlTask.StopTask();


				await Parallel.ForEachAsync(data, async (datum, _) =>
				{
					await Add(datum, ctx);
				});
			});
		}

		private FileData GetFileData()
		{
			using var stream = File.OpenText(FileDataPath);

			var serializer = new JsonSerializer();

			using var jsonTextReader = new JsonTextReader(stream);

			return serializer.Deserialize<FileData>(jsonTextReader)!;
		}


		private async Task Add(FuelStationData station, ProgressContext ctx)
		{

			var addTask = ctx.AddTask($"Adding {station.Id}");

			await Parallel.ForEachAsync(EnumHelper.GetValues<Fuel>(typeof(Fuel)), async (fuel, _) =>
			{
				var prices = station.Prices[fuel];
				if(!prices.IsEmpty)
				{
					await priceRepository.Add(station.Id, fuel, prices.Select(p => p.Date), prices.Select(p => p.Value).ToList());
				}
			});

			addTask.Description = $"Added {station.Id} prices ";

			await fuelStationRepository.Add(station.Id, station.Location, station.Services);
			addTask.Description = $"Added {station.Id} station";

			addTask.StopTask();
		}
	}
}
