using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports.FuelStation;
using Transport.Api.Db.Repositories.Internal;

namespace Transport.Api.Db.Repositories;

public class PriceRepository : BaseRepository<PriceEntity>, IPriceRepository
{
    public PriceRepository(IConfiguration configuration, ILogger<PriceRepository> logger) : base(configuration, logger)
    {
        InitIndexes();
    }


    private void InitIndexes()
    {
        CreateIndexIfMissing(nameof(PriceEntity.IdStation));
        CreateIndexIfMissing(nameof(PriceEntity.Date));
    }


    public async Task<PriceEntity> Add(long idStation, Fuel fuel, DateTime date, double value)
    {
        var elem = new PriceEntity
        {
            IdStation = idStation,
            Fuel = fuel,
            Date = date,
            Value = value
        };

        await EntityCollection.InsertOneAsync(elem);

        return elem;
    }


    public async Task<PriceEntity> GetById(string id)
    {
        return await EntityCollection.AsQueryable().Where(price => price.Id == new ObjectId(id)).FirstAsync();
    }

    public async Task<List<PriceEntity>> GetByYear(int year)
    {
        var filter = Builders<PriceEntity>.Filter.Gt(e => e.Date, new DateTime(year, 1, 1));
        filter &= Builders<PriceEntity>.Filter.Lt(e => e.Date, new DateTime(year, 12, 30));

        return await EntityCollection.FindAsync(filter).Result.ToListAsync();
    }

    public async Task<List<PriceEntity>> GetPricesByYearForStations(int year, IEnumerable<long> stationsIds)
    {
        var filter = Builders<PriceEntity>.Filter.Gt(e => e.Date, new DateTime(year, 1, 1));
        filter &= Builders<PriceEntity>.Filter.Lt(e => e.Date, new DateTime(year, 12, 30));

        filter &= Builders<PriceEntity>.Filter.In(e => e.IdStation, stationsIds);

        return await EntityCollection.FindAsync(filter).Result.ToListAsync();
    }

    public async Task<List<PriceEntity>> GetBetweenDates(DateTime minDate, DateTime maxDate)
    {
        var prices = await EntityCollection.AsQueryable().Where(fuel => fuel.Date >= minDate && fuel.Date <= maxDate).ToListAsync();

        return prices;
    }


    public async Task Clear()
    {
        await EntityCollection.Database.DropCollectionAsync(CollectionName);
        await EntityCollection.Database.CreateCollectionAsync(CollectionName);
        InitIndexes();
    }

    public async Task<List<PriceEntity>> Add(IEnumerable<FuelStationData> stations)
    {
        var entities = new List<PriceEntity>();

        foreach (var station in stations)
            foreach (Fuel fuel in Enum.GetValues(typeof(Fuel)))
            {
                var prices = station.Prices[fuel];

                foreach (var price in prices)
                    entities.Add(new PriceEntity
                    {
                        IdStation = station.Id,
                        Fuel = fuel,
                        Value = price.Value / 1000,
                        Date = price.Date
                    }
                    );
            }

        await EntityCollection.InsertManyAsync(entities);


        return entities;
    }

    public async Task<long> Clear(int year)
    {
        var result = await EntityCollection.DeleteManyAsync(price => price.Date >= new DateTime(year, 1, 1) && price.Date <= new DateTime(year, 12, 30));

        return result.DeletedCount;
    }
}