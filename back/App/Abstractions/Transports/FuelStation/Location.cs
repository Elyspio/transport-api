using System.ComponentModel.DataAnnotations;

namespace Transport.Api.Abstractions.Transports.FuelStation;

public class Location : IEquatable<Location?>
{
    [Required] public double Latitude { get; set; }

    [Required] public double Longitude { get; set; }

    [Required] public string PostalCode { get; set; }

    [Required] public string Address { get; set; }

    [Required] public string City { get; set; }

    public bool Equals(Location? other)
    {
        return other != null && Latitude == other.Latitude && Longitude == other.Longitude && PostalCode == other.PostalCode && Address == other.Address && City == other.City;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Location);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude, PostalCode, Address, City);
    }

    public static bool operator ==(Location? left, Location? right)
    {
        return EqualityComparer<Location>.Default.Equals(left, right);
    }

    public static bool operator !=(Location? left, Location? right)
    {
        return !(left == right);
    }
}