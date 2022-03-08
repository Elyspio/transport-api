using Abstraction.Enums;
using System.ComponentModel.DataAnnotations;

namespace Abstraction.Models
{
    public partial class FuelStationData
    {
        public FuelStationData(FuelStationData all)
        {
            Id = all.Id;
            Location = all.Location;
            Prices = all.Prices;
            Services = all.Services;
        }

        public FuelStationData() { }

        [Required]
        public long Id { get; set; }

        [Required]
        public Location Location { get; set; }


        [Required]
        public Prices Prices { get; set; }

        [Required]
        public List<FuelStationServiceType> Services { get; set; }

    }


    public class FuelStationHistory
    {
        public long Id { get; set; }
        public Location Location { get; set; }
        public Dictionary<Fuel, List<FuelPriceHistory>> Prices { get; set; }
    }

    public class FuelPriceHistory
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }

}
