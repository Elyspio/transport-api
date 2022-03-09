using Abstractions.Enums;
using Abstractions.Models;

namespace Abstractions.Interfaces.Repositories
{
    public interface IFuelStationRepository
    {
        Task<FuelStationData> Add(long id, Location location, Prices prices, List<FuelStationServiceType> services);
        Task Clear();
        Task<FuelStationData> GetById(long id);
        Task<List<FuelStationData>> GetBetweenDates(DateTime minDate, DateTime maxDate);
    }
}