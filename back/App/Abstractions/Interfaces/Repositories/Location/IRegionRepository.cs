using Transport.Api.Abstractions.Models.Location;

namespace Transport.Api.Abstractions.Interfaces.Repositories.Location;

public interface IRegionRepository
{
	Task<List<RegionEntity>> Add(IEnumerable<(string regionName, string regionCode)> data);
	Task Clear();
	Task<List<RegionEntity>> GetAll();
}