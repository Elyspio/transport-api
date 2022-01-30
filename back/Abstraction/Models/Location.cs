using System.ComponentModel.DataAnnotations;

namespace Abstraction.Models
{
    public class Location
    {
        [Required]
        public string Latitude { get; set; }

        [Required]
        public string Longitude { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

    }
}
