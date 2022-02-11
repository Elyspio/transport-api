using Abstraction.Enums;
using System.ComponentModel.DataAnnotations;

namespace Abstraction.Models
{
    public partial class FuelStationDataDistance: FuelStationData
    {
        public FuelStationDataDistance(FuelStationData all, double distance): base(all)
        {
            Distance = distance;
        }

        [Required]
        public double Distance { get; set; }

    }
}
