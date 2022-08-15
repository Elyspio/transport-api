using System.ComponentModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Models;
using Transport.Api.Db.Repositories.Internal;

namespace Transport.Api.Db.Repositories;

public class LocationRepository : BaseRepository<LocationEntity>, ILocationRepository
{
	public LocationRepository(IConfiguration configuration, ILogger<BaseRepository<LocationEntity>> logger) : base(configuration, logger)
	{
	}

	public async Task<LocationEntity> Add(string regionName, string regionCode, List<Departement> departements)
	{
		var id = regionCode switch
		{
			"01" => Region.Guadeloupe,
			"02" => Region.Martinique,
			"03" => Region.Guyane,
			"04" => Region.LaReunion,
			"06" => Region.Mayotte,
			"11" => Region.IleDeFrance,
			"24" => Region.CentreValDeLoire,
			"27" => Region.BourgogneFrancheComte,
			"28" => Region.Normandie,
			"32" => Region.HautDeFrance,
			"44" => Region.GrandEst,
			"52" => Region.PaysDeLaLoire,
			"53" => Region.Bretagne,
			"75" => Region.NouvelleAquitaine,
			"76" => Region.Occitanie,
			"84" => Region.AuvergneRhoneAlpes,
			"93" => Region.ProvenceAlpesCoteAzur,
			"94" => Region.Corse,
			_ => throw new ArgumentOutOfRangeException(nameof(regionCode), regionCode, null)
		};

		var entity = new LocationEntity
		{
			Id = id,
			Departements = departements,
			Code = regionCode,
			Label = regionName
		};

		await EntityCollection.InsertOneAsync(entity);

		return entity;
	}

	public async Task<List<Departement>> GetDepartements(Region region)
	{
		var entity = await EntityCollection.AsQueryable().FirstOrDefaultAsync(x => x.Id == region);

		if (entity == default) throw new InvalidEnumArgumentException("region", (int) region, typeof(Region));
		return entity.Departements;
	}

	public async Task<List<Departement>> GetAllDepartements()
	{
		return await EntityCollection.AsQueryable().SelectMany(x => x.Departements).ToListAsync();
	}


	public async Task Clear()
	{
		await EntityCollection.Database.DropCollectionAsync(CollectionName);
	}

	public async Task<List<LocationEntity>> GetRegions()
	{
		return await EntityCollection.AsQueryable().ToListAsync();
	}
}