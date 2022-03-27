using System.ComponentModel.DataAnnotations;

namespace Transport.Api.Abstractions.Transports;

public class FuelStationDataDistance : FuelStationData
{
    public FuelStationDataDistance(FuelStationData all, double distance) : base(all)
    {
        Distance = distance;
    }

    [Required] public double Distance { get; set; }
}