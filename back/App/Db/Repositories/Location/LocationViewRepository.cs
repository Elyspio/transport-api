using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Transport.Api.Abstractions.Common.Helpers;
using Transport.Api.Abstractions.Interfaces.Repositories.Location;
using Transport.Api.Abstractions.Models.Location;
using Transport.Api.Db.Repositories.Internal;

namespace Transport.Api.Db.Repositories.Location;

internal class LocationViewRepository : BaseRepository<LocationEntity>, ILocationViewRepository
{
	private readonly ILogger<BaseRepository<LocationEntity>> logger;

	public LocationViewRepository(IConfiguration configuration, ILogger<BaseRepository<LocationEntity>> logger) : base(configuration, logger, "Location")
	{
		this.logger = logger;
	}


	public async Task<List<LocationEntity>> GetAll()
	{
		var log = logger.Enter();
		var data = await EntityCollection.AsQueryable().ToListAsync();
		log.Exit();
		return data;
	}
}