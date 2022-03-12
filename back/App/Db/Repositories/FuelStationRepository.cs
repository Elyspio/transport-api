using Abstractions.Enums;
using Abstractions.Interfaces.Repositories;
using Abstractions.Models;
using Db.Entities;
using Db.Repositories.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Db.Repositories;

public class FuelStationRepository : BaseRepository<FuelStationEntity>, IFuelStationRepository
{
    public FuelStationRepository(IConfiguration configuration, ILogger<FuelStationRepository> logger) : base(configuration, logger)
    {
    }

    public async Task<FuelStationEntity> Add(long id, Location location, List<FuelStationServiceType> services)
    {
        var elem = new FuelStationEntity
        {
            Location = location,
            Id = id,
            Services = services
        };

        await EntityCollection.InsertOneAsync(elem);

        return elem;
    }

    public async Task<FuelStationEntity> GetById(long id)
    {
        return await EntityCollection.AsQueryable().Where(station => station.Id == id).FirstAsync();
    }

    public async Task<List<FuelStationEntity>> GetById(List<long> ids)
    {
        return await EntityCollection.AsQueryable().Where(station => ids.Contains(station.Id)).ToListAsync();
    }

    public async Task Clear()
    {
        await EntityCollection.Database.DropCollectionAsync(CollectionName);
    }
}