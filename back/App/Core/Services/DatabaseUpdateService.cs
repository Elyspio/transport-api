using Microsoft.Extensions.Logging;
using Spectre.Console;
using Transport.Api.Abstractions.Common.Helpers;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Interfaces.Repositories.Location;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Transports.FuelStation;
using Transport.Api.Adapters.FuelStation;
using Transport.Api.Adapters.Locations;

namespace Transport.Api.Core.Services;

public class DatabaseUpdateService : IDatabaseUpdateService
{
	private readonly ICityRepository cityRepository;
	private readonly IDepartementRepository departementRepository;
	private readonly FuelStationClient fuelStationClient;
	private readonly IFuelStationRepository fuelStationRepository;
	private readonly LocationClient locationClient;
	private readonly ILogger<DatabaseUpdateService> logger;
	private readonly IPriceRepository priceRepository;
	private readonly IRegionRepository regionRepository;
	private readonly IStatsService statsService;

	public DatabaseUpdateService(IPriceRepository priceRepository, IFuelStationRepository fuelStationRepository, FuelStationClient fuelStationClient,
		ILogger<DatabaseUpdateService> logger, IRegionRepository regionRepository, LocationClient locationClient, IDepartementRepository departementRepository, ICityRepository cityRepository,
		IStatsService statsService)
	{
		this.priceRepository = priceRepository;
		this.fuelStationRepository = fuelStationRepository;
		this.fuelStationClient = fuelStationClient;
		this.logger = logger;
		this.regionRepository = regionRepository;
		this.locationClient = locationClient;
		this.departementRepository = departementRepository;
		this.cityRepository = cityRepository;
		this.statsService = statsService;
	}

	public async Task RefreshYearly(int year)
	{
		await AnsiConsole.Progress()
			.AutoClear(false) // Do not remove the task list when done
			.Columns(new TaskDescriptionColumn(), new ProgressBarColumn(), new PercentageColumn(), new ElapsedTimeColumn(), new SpinnerColumn())
			.StartAsync(async ctx =>
			{
				var task = ctx.AddTask($"Removing {year} prices");
				var nbRemoved = await priceRepository.Clear(year);
				task.Description += $" ({nbRemoved})";
				task.Value = 100;
				task.StopTask();


				task = ctx.AddTask($"Fetching fuel stations for {year}");
				var data = await fuelStationClient.GetFuelStationsByYear(year);
				task.Description += $" ({data.Count})";
				task.Value = 100;
				task.StopTask();

				task = ctx.AddTask("Updating database");
				await Task.WhenAll(UpdatePriceEntities(data), UpdateStationEntities(data));
				task.Value = 100;
				task.StopTask();

				task = ctx.AddTask("Refresh weekly statistics", new ProgressTaskSettings { MaxValue = 52}) ;
				await statsService.RefreshWeeklyStats(year, task);
				task.StopTask();

				if (year == DateTime.Now.Year) await statsService.RefreshDailyStats(ctx);
			});
	}


	public async Task RefreshLocations()
	{
		var log = logger.Enter();
		await regionRepository.Clear();
		await departementRepository.Clear();
		await cityRepository.Clear();

		var regions = await locationClient.GetRegions();
		var departements = await locationClient.GetDepartements();
		var rawCities = await locationClient.GetCities();


		var regionEntities = await regionRepository.Add(regions.Select(region => (region.Nom, region.Code)));
		var departmentEntities = await departementRepository.Add(departements.Select(dep => (dep.Name, dep.Code, regionEntities.First(region => region.Code == dep.CodeRegion).Id)));

		var cities = rawCities.Select(city =>
		{
			var postaleCode = city.CodesPostaux.First();
			return (city.Nom, postaleCode, departmentEntities.First(dep => postaleCode.StartsWith(dep.Code)).Id);
		}).ToList();


		await cityRepository.Add(cities);

		log.Exit();
	}


	private async Task UpdatePriceEntities(List<FuelStationData> data)
	{
		var prices = await priceRepository.Add(data);
		logger.LogInformation($"Added {prices.Count} prices");
	}


	private async Task UpdateStationEntities(List<FuelStationData> data)
	{
		var existing = await fuelStationRepository.GetAllIds();

		var toAdd = data.Where(station => !existing.Contains(station.Id)).ToList();

		if (toAdd.Any())
		{
			var updated = await fuelStationRepository.Add(toAdd);
			logger.LogInformation($"Added {updated.Count} stations");
		}
	}
}