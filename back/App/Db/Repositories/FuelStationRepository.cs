using Abstractions.Enums;
using Abstractions.Interfaces.Repositories;
using Abstractions.Models;
using Db.Assemblers;
using Db.Entities;
using Db.Repositories.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Db.Repositories;

public class FuelStationRepository : BaseRepository<FuelStationEntity>, IFuelStationRepository
{
    private readonly FuelStationAssembler assembler;

    public FuelStationRepository(IConfiguration configuration, ILogger<FuelStationRepository> logger) : base(configuration, logger)
    {
        assembler = new FuelStationAssembler();
    }

    public async Task<FuelStationData> Add(long id, Location location, Prices prices, List<FuelStationServiceType> services)
    {
        var elem = new FuelStationEntity
        {
            Prices = prices,
            Location = location,
            Id = id,
            Services = services
        };

        await EntityCollection.InsertOneAsync(elem);

        return elem;
    }


    public async Task<FuelStationData> GetById(long id)
    {
        var station = await EntityCollection.AsQueryable().Where(station => station.Id == id).FirstAsync();
        return assembler.Convert(station); ;
    }


    public async Task<List<FuelStationData>> GetBetweenDates(DateTime minDate, DateTime maxDate)
    {
        var stations = await EntityCollection.AsQueryable().ToListAsync();

        stations.ForEach(station =>
        {
            var prices = new Prices();
            foreach (Fuel fuel in Enum.GetValues(typeof(Fuel)))
            {
                station.Prices[fuel].ForEach(price =>
                {
                    if (price.Date > minDate && price.Date < maxDate)
                    {
                        prices[fuel].Add(price);
                    }
                });
            }
            station.Prices = prices;
        });

        return assembler.Convert(stations).ToList();
    }


    public async Task Clear()
    {
        await EntityCollection.Database.DropCollectionAsync(CollectionName);
    }

}