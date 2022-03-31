using Microsoft.Extensions.Logging;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Transports;
using Transport.Api.Adapters.FuelStation;

namespace Transport.Api.Core.Services;

public class DatabaseUpdateService : IDatabaseUpdateService
{
    private readonly FuelStationClient fuelStationClient;
    private readonly IFuelStationRepository fuelStationRepository;
    private readonly ILogger<DatabaseUpdateService> logger;
    private readonly IPriceRepository priceRepository;

    public DatabaseUpdateService(IPriceRepository priceRepository, IFuelStationRepository fuelStationRepository,
        FuelStationClient fuelStationClient, ILogger<DatabaseUpdateService> logger)
    {
        this.priceRepository = priceRepository;
        this.fuelStationRepository = fuelStationRepository;
        this.fuelStationClient = fuelStationClient;
        this.logger = logger;
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
}