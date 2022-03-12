using Abstractions.Enums;

namespace Abstractions.Models
{
    public class FuelStationHistory
    {
        public long Id { get; set; }
        public Location Location { get; set; }
        public Dictionary<Fuel, List<FuelPriceHistory>> Prices { get; set; }
    }

}
