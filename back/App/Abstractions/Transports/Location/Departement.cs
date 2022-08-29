using System.ComponentModel.DataAnnotations;

namespace Transport.Api.Abstractions.Transports.Location;

public class Departement
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public string Code { get; set; }

	[Required]
	public List<City> Cities { get; set; }
}