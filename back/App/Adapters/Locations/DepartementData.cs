using Newtonsoft.Json;

namespace Transport.Api.Adapters.Locations;

public class DepartementData
{
	[JsonProperty("nom")] public string Name { get; set; } = default!;

	[JsonProperty("code")] public string Code { get; set; } = default!;

	[JsonProperty("codeRegion")] public string CodeRegion { get; set; } = default!;
}