using System.Collections.Concurrent;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Transports.FuelStation;
using Transport.Api.Adapters.FuelStation;
using Transport.Api.Core.Assemblers;

namespace Transport.Api.Core.Services;

public class FuelStationService : IFuelStationService
{
    private readonly FuelStationClient client;
    private readonly FuelStationAssembler fuelStationAssembler = new();
    private readonly IPriceRepository priceRepository;
    private readonly IFuelStationRepository stationRepository;

    public FuelStationService(FuelStationClient client, IFuelStationRepository repository, IPriceRepository priceRepository)
    {
        this.client = client;
        stationRepository = repository;
        this.priceRepository = priceRepository;
    }

    public async Task<List<FuelStationDataDistance>> GetFuelStations(double lat, double lon, long radius)
    {
        var stations = await client.GetFuelStations();

        return stations.Select(station => new FuelStationDataDistance(station, GetDistance(lat, lon, station.Location.Latitude, station.Location.Longitude)))
            .Where(station => station.Distance < radius * 1000)
            .ToList();
    }

    public async Task<List<FuelStationData>> GetBetweenDates(DateTime? minDate = null, DateTime? maxDate = null)
    {
        minDate ??= DateTime.MinValue;

        maxDate ??= DateTime.MaxValue;

        var allPrices = await priceRepository.GetBetweenDates(minDate.Value, maxDate.Value);
        Console.WriteLine($"Count prices {allPrices.Count}");

        var stationIds = allPrices.Select(p => p.IdStation).Distinct().ToList();

        var stations = await stationRepository.GetById(stationIds);
        Console.WriteLine($"Count stations {stations.Count}");

        var datas = new ConcurrentBag<FuelStationData>();

        Parallel.ForEach(stations, station =>
            {
                var data = fuelStationAssembler.Convert(station);

                var prices = allPrices.Where(p => p.IdStation == station.Id).ToList();

                foreach (Fuel fuel in Enum.GetValues(typeof(Fuel)))
                    data.Prices[fuel]
                        .AddRange(prices.Where(p => p.Fuel == fuel)
                            .Select(p => new FuelPriceHistory
                                {
                                    Date = p.Date,
                                    Value = p.Value
                                }
                            )
                            .ToList()
                        );


                datas.Add(data);
            }
        );


        return datas.ToList();
    }

    private double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var R = 6378.137; // Radius of earth in KM
        var dLat = lat2 * Math.PI / 180 - lat1 * Math.PI / 180;
        var dLon = lon2 * Math.PI / 180 - lon1 * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var d = R * c;
        return d * 1000; // meters
    }
}