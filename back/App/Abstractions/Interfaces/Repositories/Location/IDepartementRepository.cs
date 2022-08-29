using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Models.Location;

namespace Transport.Api.Abstractions.Interfaces.Repositories.Location;

public interface IDepartementRepository
{
	Task<List<DepartementEntity>> Add(IEnumerable<(string name, string code, RegionId regionIdId)> data);
	Task Clear();
	Task<List<DepartementEntity>> GetAll();
	Task<List<DepartementEntity>> GetByRegion(RegionId region);
}