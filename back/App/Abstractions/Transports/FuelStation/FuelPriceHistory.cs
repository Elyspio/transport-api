using System.ComponentModel.DataAnnotations;

namespace Transport.Api.Abstractions.Transports.FuelStation;

public class FuelPriceHistory
{
    [Required] public DateTime Date { get; set; }

    [Required] public double Value { get; set; }
}