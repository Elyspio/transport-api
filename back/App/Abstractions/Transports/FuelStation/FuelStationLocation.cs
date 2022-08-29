using System.ComponentModel.DataAnnotations;

namespace Transport.Api.Abstractions.Transports.FuelStation;

public class FuelStationLocation : IEquatable<FuelStationLocation?>
{
	[Required] public double Latitude { get; set; } = default!;

	[Required] public double Longitude { get; set; } = default!;

	[Required] public string PostalCode { get; set; } = default!;

	[Required] public string Address { get; set; } = default!;

	[Required] public string City { get; set; } = default!;

	public bool Equals(FuelStationLocation? other)
	{
		return other != null && Latitude == other.Latitude && Longitude == other.Longitude && PostalCode == other.PostalCode && Address == other.Address &&
				City == other.City;
	}

	public override bool Equals(object? obj)
	{
		return Equals(obj as FuelStationLocation);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Latitude, Longitude, PostalCode, Address, City);
	}

	public static bool operator ==(FuelStationLocation? left, FuelStationLocation? right)
	{
		return EqualityComparer<FuelStationLocation>.Default.Equals(left, right);
	}

	public static bool operator !=(FuelStationLocation? left, FuelStationLocation? right)
	{
		return !(left == right);
	}
}