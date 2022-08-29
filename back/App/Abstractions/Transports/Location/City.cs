using System.ComponentModel.DataAnnotations;

namespace Transport.Api.Abstractions.Transports.Location;

public class City
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public string PostalCode { get; set; }
}