using Microsoft.Extensions.Logging;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports.FuelStation;
using Transport.Api.Adapters.FuelStation;
using Transport.Api.Adapters.Locations;

namespace Transport.Api.Core.Services;

public class DatabaseUpdateService : IDatabaseUpdateService
{
    private readonly FuelStationClient fuelStationClient;
    private readonly LocationClient locationClient;
    private readonly IFuelStationRepository fuelStationRepository;
    private readonly ILogger<DatabaseUpdateService> logger;
    private readonly IPriceRepository priceRepository;
    private readonly ILocationRepository departementRepository;

    public DatabaseUpdateService(IPriceRepository priceRepository, IFuelStationRepository fuelStationRepository, FuelStationClient fuelStationClient,
        ILogger<DatabaseUpdateService> logger, ILocationRepository departementRepository, LocationClient locationClient)
    {
        this.priceRepository = priceRepository;
        this.fuelStationRepository = fuelStationRepository;
        this.fuelStationClient = fuelStationClient;
        this.logger = logger;
        this.departementRepository = departementRepository;
        this.locationClient = locationClient;
    }

    public async Task RefreshYearly(int year)
    {
        var nbRemoved = await priceRepository.Clear(year);
        logger.LogInformation($"Removed {nbRemoved} prices for the year {year}");

        var data = await fuelStationClient.GetFuelStationsByYear(year);
        logger.LogInformation($"Received {data.Count} stations for the year {year}");
        await Task.WhenAll(UpdatePriceEntities(data), UpdateStationEntities(data));
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


    public async Task RefreshLocations()
    {
        await departementRepository.Clear();

        var regions = await locationClient.GetRegions();
        var departements = await locationClient.GetDepartements();

        await Task.WhenAll(regions.Select(region =>
        {
            var deps = departements
                .Where(departement => departement.CodeRegion == region.Code)
                .Select(departement => new Departement { Code = departement.Code, Name = departement.Name })
                .ToList();

            return departementRepository.Add(region.Nom, region.Code, deps);
        }).ToArray());

    }
}