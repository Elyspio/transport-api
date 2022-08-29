using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories.Location;
using Transport.Api.Abstractions.Models.Location;
using Transport.Api.Db.Repositories.Internal;

namespace Transport.Api.Db.Repositories.Location;

internal class DepartementRepository : BaseRepository<DepartementEntity>, IDepartementRepository
{
	public DepartementRepository(IConfiguration configuration, ILogger<BaseRepository<DepartementEntity>> logger) : base(configuration, logger, "Location.Departements")
	{
		CreateIndexes();
	}

	public async Task<List<DepartementEntity>> Add(IEnumerable<(string name, string code, RegionId regionIdId)> data)
	{
		var entities = data.Select(datum => new DepartementEntity
		{
			Code = datum.code,
			Name = datum.name,
			RegionId = datum.regionIdId
		}).ToList();

		await EntityCollection.InsertManyAsync(entities, new InsertManyOptions { IsOrdered = false });

		return entities;
	}

	public async Task Clear()
	{
		await EntityCollection.Database.DropCollectionAsync(CollectionName);
		CreateIndexes();
	}

	public async Task<List<DepartementEntity>> GetAll()
	{
		return await EntityCollection.AsQueryable().ToListAsync();
	}

	public async Task<List<DepartementEntity>> GetByRegion(RegionId region)
	{
		return await EntityCollection.AsQueryable().Where(dep => dep.RegionId == region).ToListAsync();
	}


	private void CreateIndexes()
	{
		CreateIndexIfMissing(new List<string> { nameof(DepartementEntity.RegionId) });
	}
}