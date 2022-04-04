using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Models;
using Transport.Api.Abstractions.Transports;

namespace Transport.Api.Abstractions.Interfaces.Repositories;

public interface IFuelStationRepository
{
    Task<FuelStationEntity> Add(long id, Location location, List<FuelStationServiceType> services);
    Task<List<FuelStationEntity>> Add(IEnumerable<FuelStationData> stations);
    Task<List<long>> GetAllIds();

    Task Clear();
    Task<List<FuelStationEntity>> GetById(IEnumerable<long> ids);
    Task<FuelStationEntity> GetById(long id);
}