using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports.Location;

namespace Transport.Api.Abstractions.Interfaces.Services;

public interface ILocationService
{
	public Task<List<Departement>> GetDepartements(Region region);
	public Task<List<Departement>> GetAllDepartements();

	public Task<List<RegionTransport>> GetRegions();
}