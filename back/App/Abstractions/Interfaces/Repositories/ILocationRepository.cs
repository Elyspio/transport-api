using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Models;

namespace Transport.Api.Abstractions.Interfaces.Repositories;

public interface ILocationRepository
{
	Task<LocationEntity> Add(string regionName, string regionCode, List<Departement> departements);
	Task<List<Departement>> GetDepartements(Region region);
	Task<List<Departement>> GetAllDepartements();
	Task Clear();
	Task<List<LocationEntity>> GetRegions();
}