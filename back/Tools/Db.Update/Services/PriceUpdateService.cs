using Spectre.Console;
using Transport.Api.Abstractions.Transports.FuelStation;
using Transport.Api.Adapters.FuelStation;
using Transport.Api.Db.Repositories;

namespace Transport.Api.Db.Update.Services;

internal class PriceUpdateService
{
	private readonly PriceRepository fuelImportRepository;
	private readonly FuelStationClient fuelStationClient;
	private readonly FuelStationRepository fuelStationImportRepository;

	public PriceUpdateService(FuelStationRepository fuelStationImportRepository, PriceRepository fuelImportRepository, FuelStationClient fuelStationClient)
	{
		this.fuelStationImportRepository = fuelStationImportRepository;
		this.fuelImportRepository = fuelImportRepository;
		this.fuelStationClient = fuelStationClient;
	}

	public async Task UpdatePrices(int year)
	{
		await AnsiConsole.Progress()
			.AutoClear(false)
			.Columns(new TaskDescriptionColumn { Alignment = Justify.Left }, new ElapsedTimeColumn(), new SpinnerColumn())
			.StartAsync(async ctx =>
			{
				var removeTask = ctx.AddTask($"Removing {year}'s prices");
				var nbRemoved = await fuelImportRepository.Clear(year);
				removeTask.StopTask();
				removeTask.Description = $"Removed {nbRemoved} prices for {year}";

				var downloadTask = ctx.AddTask($"Downloading fuel stations for the year {year}");
				var data = await fuelStationClient.GetFuelStationsByYear(year);
				downloadTask.StopTask();
				downloadTask.Description = $"Downloaded {data.Count} fuel stations";


				await Task.WhenAll(new List<Task>
						{
							UpdateStationEntities(ctx, data),
							UpdatePriceEntities(ctx, data)
						}
				);
			}
			);
	}

	private async Task UpdatePriceEntities(ProgressContext ctx, List<FuelStationData> data)
	{
		var addTask = ctx.AddTask("Inserting fuel prices");
		var prices = await fuelImportRepository.Add(data);
		addTask.StopTask();
		addTask.Description = $"{prices.Count} prices have been inserted";
	}


	private async Task UpdateStationEntities(ProgressContext ctx, List<FuelStationData> data)
	{
		var existings = await fuelStationImportRepository.GetAllIds();

		var toAdd = data.Where(station => !existings.Contains(station.Id)).ToList();

		var updateStationsTask = ctx.AddTask("Updating fuel stations");

		if (toAdd.Any())
		{
			var updated = await fuelStationImportRepository.Add(toAdd);
			updateStationsTask.Description = $"{updated.Count} fuel stations have been updated";
		}

		updateStationsTask.StopTask();
		updateStationsTask.Description = "No fuel station have been updated";
	}


	public async Task ClearAll()
	{
		await AnsiConsole.Progress()
			.AutoClear(false)
			.Columns(new TaskDescriptionColumn { Alignment = Justify.Left }, new ElapsedTimeColumn(), new SpinnerColumn())
			.StartAsync(async ctx =>
			{
				var clearTask = ctx.AddTask("Clearing fuel and station collections");
				await Task.WhenAll(fuelImportRepository.Clear(), fuelStationImportRepository.Clear());

				clearTask.StopTask();
				clearTask.Description = "Cleared fuel and station collections ";
			}
			);
	}
}