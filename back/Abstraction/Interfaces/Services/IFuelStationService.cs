using Abstraction.Models;

namespace Abstraction.Interfaces.Services;

public interface IFuelStationService
{
    Task<List<FuelStationData>> GetFuelStations();
}
