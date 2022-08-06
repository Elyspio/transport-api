using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Spectre.Console;
using System.Collections.Concurrent;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Transports.FuelStation;
using Transport.Api.Adapters;
using Transport.Api.Adapters.FuelStation;

namespace Transport.Api.Db.Cache.Services
{
	internal class PriceCacheService
	{
		private const string Location = "/internal/transport-api";
		private const string Name = "merged.json";
		private readonly FuelStationClient fuelStationClient;
		private readonly PublicFilesClient publicFiles;
		private readonly ILogger<PriceCacheService> logger;
		private readonly FuelStationAssembler stationAssembler = new();
		public PriceCacheService(FuelStationClient fuelStationClient, PublicFilesClient publicFiles, ILogger<PriceCacheService> logger)
		{
			this.fuelStationClient = fuelStationClient;
			this.publicFiles = publicFiles;
			this.logger = logger;
		}

		private async Task<List<FuelStationData>> Fetch(ProgressContext ctx, int year)
		{
			var task = ctx.AddTask($"{year} - Fetching ");
			var xml = await fuelStationClient.Download(year);
			task.Description = $"{year} - Fetched ";

			task.Description = ($"{year} - Parsing ");
			var data = fuelStationClient.Parse(xml);
			xml = null;
			GC.Collect();
			task.Description = $"{year} - Parsed";


			task.Description = ($"{year} - Converting");
			var converted = stationAssembler.Convert(data);
			data = null;
			GC.Collect();
			task.Description = $"{year} - Converted";

			task.StopTask();

			return converted;

		}


		public async Task<FileData> Create(List<int> years)
		{

			return await AnsiConsole.Progress()
			.AutoClear(false)
			.Columns(new TaskDescriptionColumn { Alignment = Justify.Left }, new ElapsedTimeColumn(), new SpinnerColumn())
			.StartAsync(async ctx =>
			{
				List<List<FuelStationData>>? allRawStations;

				{
					var splitData = new ConcurrentBag<List<FuelStationData>>();

					await Parallel.ForEachAsync(years, new ParallelOptions { MaxDegreeOfParallelism = 8 }, async (year, _) =>
					{
						var stations = await Fetch(ctx, year);
						splitData.Add(stations);
					});

					allRawStations = splitData.ToList();
					splitData = null;
				}


				var allStations = new ConcurrentDictionary<long, FuelStationData>();


				ICollection<FuelStationData>? merged;


				{
					Parallel.ForEach(allRawStations, (content, _, index) =>
					{
						var year = years[(int) index];
						var concatTask = ctx.AddTask($"Concating {year}");
						Parallel.ForEach(content, pdv =>
						{
							if (allStations.TryGetValue(pdv.Id, out var item))
							{
								foreach (Fuel fuel in Enum.GetValues(typeof(Fuel)))
								{
									foreach (var price in pdv.Prices[fuel])
									{
										item.Prices[fuel].Add(price);
									}
								}
								var set = new HashSet<FuelStationServiceType>();
								item.Services.ForEach(service => set.Add(service));
								pdv.Services.ForEach(service => set.Add(service));
								item.Services = set.ToList();
							}
							else
							{
								item = pdv;
							}
							allStations[pdv.Id] = item;
						});

						concatTask.StopTask();
						concatTask.Description = $"Concacted {year}";

					});


					merged = allStations.Values;
				}


				allRawStations = null;


				var serializeTask = ctx.AddTask($"Extracting");

				var tempFile = "P:\\own\\mobile\\transport-api\\back\\Tools\\Db.Cache\\merged.json";

				using (var fileTemp = File.CreateText(tempFile))
				{
					var serializer = new JsonSerializer() { Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }, };
					serializer.Serialize(fileTemp, merged);
				}
				merged = null;
				serializeTask.StopTask();
				serializeTask.Description = $"Extracted";


				var downloadTask = ctx.AddTask($"Uploading");
				using var stream = File.OpenRead(tempFile);
				var file = await publicFiles.AddFileAsync(Name, Location, new FileParameter(stream, Name, "application/json"), true);
				downloadTask.StopTask();
				downloadTask.Description = $"Uploaded";

				return file;
			});

		}
	}
}
