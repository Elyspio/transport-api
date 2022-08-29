using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories.Location;
using Transport.Api.Abstractions.Models.Location;
using Transport.Api.Db.Repositories.Internal;

namespace Transport.Api.Db.Repositories.Location;

public class RegionRepository : BaseRepository<RegionEntity>, IRegionRepository
{
	private const string regionsKey = "departements";

	private readonly IMemoryCache cache;

	public RegionRepository(IConfiguration configuration, ILogger<BaseRepository<RegionEntity>> logger, IMemoryCache cache) : base(configuration, logger, "Location.Regions")
	{
		this.cache = cache;
	}

	public async Task<List<RegionEntity>> Add(IEnumerable<(string regionName, string regionCode)> data)
	{
		var entities = data.Select(datum => Convert(datum.regionCode, datum.regionName)).ToList();

		await EntityCollection.InsertManyAsync(entities, new InsertManyOptions { IsOrdered = false });

		return entities;
	}

	public async Task Clear()
	{
		await EntityCollection.Database.DropCollectionAsync(CollectionName);
	}

	public async Task<List<RegionEntity>> GetAll()
	{
		if (cache.TryGetValue<List<RegionEntity>>(regionsKey, out var data)) return data;
		data = await EntityCollection.AsQueryable().ToListAsync();
		cache.Set(regionsKey, data);
		return data;
	}

	private RegionEntity Convert(string regionCode, string regionName)
	{
		var id = regionCode switch
		{
			"01" => RegionId.Guadeloupe,
			"02" => RegionId.Martinique,
			"03" => RegionId.Guyane,
			"04" => RegionId.LaReunion,
			"06" => RegionId.Mayotte,
			"11" => RegionId.IleDeFrance,
			"24" => RegionId.CentreValDeLoire,
			"27" => RegionId.BourgogneFrancheComte,
			"28" => RegionId.Normandie,
			"32" => RegionId.HautDeFrance,
			"44" => RegionId.GrandEst,
			"52" => RegionId.PaysDeLaLoire,
			"53" => RegionId.Bretagne,
			"75" => RegionId.NouvelleAquitaine,
			"76" => RegionId.Occitanie,
			"84" => RegionId.AuvergneRhoneAlpes,
			"93" => RegionId.ProvenceAlpesCoteAzur,
			"94" => RegionId.Corse,
			_ => throw new ArgumentOutOfRangeException(nameof(regionCode), regionCode, null)
		};

		var entity = new RegionEntity
		{
			Code = regionCode,
			Id = id,
			Label = regionName
		};

		return entity;
	}
}