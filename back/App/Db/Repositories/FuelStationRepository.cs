using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports.FuelStation;
using Transport.Api.Db.Repositories.Internal;

namespace Transport.Api.Db.Repositories;

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

	public async Task<List<FuelStationEntity>> Add(IEnumerable<FuelStationData> stations)
	{
		var entities = stations.Select(s => new FuelStationEntity
				{
					Id = s.Id,
					Location = s.Location,
					Services = s.Services
				}
			)
			.ToList();

		await EntityCollection.InsertManyAsync(entities);

		return entities;
	}


	public async Task<FuelStationEntity> GetById(long id)
	{
		return await EntityCollection.AsQueryable().Where(station => station.Id == id).FirstAsync();
	}

	public async Task<List<FuelStationEntity>> GetById(IEnumerable<long> ids)
	{
		return await EntityCollection.AsQueryable().Where(station => ids.Contains(station.Id)).ToListAsync();
	}

	public async Task<List<long>> GetAllIds()
	{
		return await EntityCollection.AsQueryable().Select(station => station.Id).ToListAsync();
	}

	public async Task Clear()
	{
		await EntityCollection.Database.DropCollectionAsync(CollectionName);
	}
}