using System.ComponentModel.DataAnnotations;
using Transport.Api.Abstractions.Enums;

namespace Transport.Api.Abstractions.Transports.Location;

public class RegionTransport
{
    [Required] public Region Id { get; set; }

    [Required] public string Code { get; set; }

    [Required] public string Label { get; set; }
}