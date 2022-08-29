using Transport.Api.Abstractions.Models.Location;

namespace Transport.Api.Abstractions.Interfaces.Repositories.Location;

public interface ILocationViewRepository
{
	Task<List<LocationEntity>> GetAll();
}