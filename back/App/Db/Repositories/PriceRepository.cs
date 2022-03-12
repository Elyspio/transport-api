using Abstractions.Enums;
using Abstractions.Interfaces.Repositories;
using Db.Entities;
using Db.Repositories.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Db.Repositories;

public class PriceRepository : BaseRepository<PriceEntity>, IPriceRepository
{

    public PriceRepository(IConfiguration configuration, ILogger<PriceRepository> logger) : base(configuration, logger)
    {
    }

    public async Task<PriceEntity> Add(long idStation, Fuel fuel, DateTime date, double value)
    {
        var elem = new PriceEntity
        {
            IdStation = idStation,
            Fuel = fuel,
            Date = date,
            Value = value,
        };

        await EntityCollection.InsertOneAsync(elem);

        return elem;
    }


    public async Task<PriceEntity> GetById(string id)
    {
        return await EntityCollection.AsQueryable().Where(price => price.Id == new ObjectId(id)).FirstAsync();
    }


    public async Task<List<PriceEntity>> GetBetweenDates(DateTime minDate, DateTime maxDate)
    {
        var prices = await EntityCollection
            .AsQueryable()
            .Where(fuel => fuel.Date >= minDate && fuel.Date <= maxDate)
            .ToListAsync();

        return prices;
    }


    public async Task Clear()
    {
        await EntityCollection.Database.DropCollectionAsync(CollectionName);
    }

}