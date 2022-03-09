using Abstractions.Enums;
using System.ComponentModel.DataAnnotations;

namespace Abstractions.Models
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

}
