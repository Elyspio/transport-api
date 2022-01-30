using Abstraction.Models;

namespace Abstraction.Interfaces.Services;

public interface IFuelStationService
{
    /// <summary>
    /// Get All fuel stations around a point
    /// </summary>
    /// <param name="latitude"></param>
    /// <param name="longitude"></param>
    /// <param name="radius">Maximal distance in meter between the fuel station and the specified point</param>
    /// <returns></returns>
    Task<List<FuelStationData>> GetFuelStations(double latitude, double longitude, long radius);
}
