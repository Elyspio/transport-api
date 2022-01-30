using Abstraction.Enums;
using System.ComponentModel.DataAnnotations;

namespace Abstraction.Models
{
    public partial class FuelStationData
    {
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
