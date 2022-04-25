using Newtonsoft.Json;

namespace Transport.Api.Adapters.Locations;

public class DepartementData
{
	[JsonProperty("nom")] public string Name { get; set; }

	[JsonProperty("code")] public string Code { get; set; }

	[JsonProperty("codeRegion")] public string CodeRegion { get; set; }
}