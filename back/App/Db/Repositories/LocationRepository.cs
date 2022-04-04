using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Transport.Api.Abstractions.Helpers;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Models;
using Transport.Api.Db.Repositories.Internal;

namespace Transport.Api.Db.Repositories;

public class LocationRepository : BaseRepository<FuelStationEntity>, ILocationRepository
{
    public LocationRepository(IConfiguration configuration, ILogger<LocationRepository> logger) : base(configuration, logger) { }

    public async Task<List<string>> GetPostalCodes()
    {
        return await EntityCollection.AsQueryable().Select(x => x.Location.PostalCode).Distinct().ToListAsync();
    }

    public async Task<List<string>> GetPostalCodes(Region region)
    {
        var departements = region.GetDepartements();

        return await EntityCollection.AsQueryable()
            .Where(x => departements.Contains(x.Location.PostalCode.Substring(0, 2)))
            .Select(x => x.Location.PostalCode)
            .Distinct()
            .ToListAsync();
    }
}