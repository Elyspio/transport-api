using Transport.Api.Abstractions.Transports.Location;

namespace Transport.Api.Abstractions.Interfaces.Services;

public interface ILocationService
{
	public Task<List<Region>> GetMergedData();
}