using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Transports.FuelStation;

public class FuelStationHistory
{
	public long Id { get; set; }
	public FuelStationLocation Location { get; set; } = default!;
	public Dictionary<Fuel, List<FuelPriceHistory>> Prices { get; set; } = default!;
}