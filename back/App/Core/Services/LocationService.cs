using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Interfaces.Services;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports.Location;

namespace Transport.Api.Core.Services;

public class LocationService : ILocationService
{
    private readonly ILocationRepository locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        this.locationRepository = locationRepository;
    }

    public async Task<List<Departement>> GetDepartements(Region region)
    {
        return await locationRepository.GetDepartements(region);
    }

    public async Task<List<Departement>> GetAllDepartements()
    {
        return await locationRepository.GetAllDepartements();
    }

    public async Task<List<RegionTransport>> GetRegions()
    {
        var regions = await locationRepository.GetRegions();

        return regions.Select(entity => new RegionTransport
                {
                    Id = entity.Id,
                    Code = entity.Code,
                    Label = entity.Label
                }
            )
            .ToList();
    }
}