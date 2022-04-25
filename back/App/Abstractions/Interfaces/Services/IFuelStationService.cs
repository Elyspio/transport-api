using Transport.Api.Abstractions.Transports.FuelStation;

namespace Transport.Api.Abstractions.Interfaces.Services;

public interface IFuelStationService
{
	/// <summary>
	///     Get All fuel stations around a point
	/// </summary>
	/// <param name="latitude"></param>
	/// <param name="longitude"></param>
	/// <param name="radius">Maximal distance in meter between the fuel station and the specified point</param>
	/// <returns></returns>
	Task<List<FuelStationDataDistance>> GetFuelStations(double latitude, double longitude, long radius);

	public Task<List<FuelStationData>> GetBetweenDates(DateTime? minDate = null, DateTime? maxDate = null);
}