using Microsoft.Extensions.Logging;
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

	public DatabaseUpdateService(IPriceRepository priceRepository, IFuelStationRepository fuelStationRepository, FuelStationClient fuelStationClient,
		ILogger<DatabaseUpdateService> logger, IRegionRepository regionRepository, LocationClient locationClient, IDepartementRepository departementRepository, ICityRepository cityRepository)
	{
		this.priceRepository = priceRepository;
		this.fuelStationRepository = fuelStationRepository;
		this.fuelStationClient = fuelStationClient;
		this.logger = logger;
		this.regionRepository = regionRepository;
		this.locationClient = locationClient;
		this.departementRepository = departementRepository;
		this.cityRepository = cityRepository;
	}

	public async Task RefreshYearly(int year)
	{
		var nbRemoved = await priceRepository.Clear(year);
		logger.LogInformation($"Removed {nbRemoved} prices for the year {year}");

		var data = await fuelStationClient.GetFuelStationsByYear(year);
		logger.LogInformation($"Received {data.Count} stations for the year {year}");
		await Task.WhenAll(UpdatePriceEntities(data), UpdateStationEntities(data));
	}


	public async Task RefreshLocations()
	{
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
			Console.WriteLine($"length {city.CodesPostaux.Count}");
			var postaleCode = city.CodesPostaux.First();
			return (city.Nom, postaleCode, departmentEntities.First(dep => postaleCode.StartsWith(dep.Code)).Id);
		}).ToList();


		await cityRepository.Add(cities);
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