using Newtonsoft.Json;

namespace Transport.Api.Adapters.Locations;

public class CityData
{
	[JsonProperty("codeRegion")] public string CodeRegion { get; set; } = null!;

	[JsonProperty("nom")] public string Nom { get; set; } = null!;

	[JsonProperty("codesPostaux")] public List<string> CodesPostaux { get; set; } = new();

	[JsonProperty("code")] public string Code { get; set; } = null!;
}