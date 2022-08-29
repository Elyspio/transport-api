using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Transport.Api.Abstractions.Interfaces.Repositories.Location;
using Transport.Api.Abstractions.Models.Location;
using Transport.Api.Db.Repositories.Internal;

namespace Transport.Api.Db.Repositories.Location;

internal class CityRepository : BaseRepository<CityEntity>, ICityRepository
{
	private readonly ILogger<BaseRepository<CityEntity>> logger;

	public CityRepository(IConfiguration configuration, ILogger<BaseRepository<CityEntity>> logger) : base(configuration, logger, "Location.Cities")
	{
		this.logger = logger;
		CreateIndexes();
	}


	public async Task<List<CityEntity>> Add(IEnumerable<(string name, string postalCode, ObjectId DepartementId)> data)
	{
		var entities = data.Select(datum => new CityEntity
		{
			Name = datum.name,
			DepartementId = datum.DepartementId,
			PostalCode = datum.postalCode
		}).ToList();

		await EntityCollection.InsertManyAsync(entities, new InsertManyOptions { IsOrdered = false });

		return entities;
	}

	public async Task Clear()
	{
		await EntityCollection.Database.DropCollectionAsync(CollectionName);
		CreateIndexes();
	}

	public async Task<List<CityEntity>> GetAll()
	{
		return await EntityCollection.AsQueryable().ToListAsync();
	}

	public async Task<List<string>> GetAllPostalCodes()
	{
		return await EntityCollection.AsQueryable().Select(city => city.PostalCode).ToListAsync();
	}

	private void CreateIndexes()
	{
		CreateIndexIfMissing(new List<string> { nameof(CityEntity.DepartementId) });
	}
}