using Abstractions.Enums;
using Abstractions.Models;
using Db.Entities;

namespace Abstractions.Interfaces.Repositories
{
    public interface IFuelStationRepository
    {
        Task<FuelStationEntity> Add(long id, Location location, List<FuelStationServiceType> services);
        Task Clear();
        Task<List<FuelStationEntity>> GetById(List<long> ids);
        Task<FuelStationEntity> GetById(long id);
    }
}