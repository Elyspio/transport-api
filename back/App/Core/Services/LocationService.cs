using Transport.Api.Abstractions.Interfaces.Repositories.Location;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Transports.Location;
using Transport.Api.Core.Assemblers;

namespace Transport.Api.Core.Services;

public class LocationService : ILocationService
{
	private readonly LocationAssembler locationAssembler = new();

	private readonly ILocationViewRepository locationViewRepository;


	public LocationService(ILocationViewRepository locationViewRepository)
	{
		this.locationViewRepository = locationViewRepository;
	}


	public async Task<List<Region>> GetMergedData()
	{
		var data = await locationViewRepository.GetAll();
		return locationAssembler.Convert(data);
	}
}