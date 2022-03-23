using System.ComponentModel.DataAnnotations;

namespace Abstractions.Models
{
    public class FuelPriceHistory
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double Value { get; set; }
    }

}
