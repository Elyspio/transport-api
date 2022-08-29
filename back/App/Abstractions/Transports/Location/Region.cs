using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Transports.Location;

public class Region
{
	[Required]
	public RegionId Id { get; set; }

	[Required]
	public string Code { get; set; }

	[Required]
	public string Label { get; set; }

	[Required]
	public List<Departement> Departements { get; set; }
}