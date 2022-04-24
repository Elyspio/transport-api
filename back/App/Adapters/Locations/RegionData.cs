using Newtonsoft.Json;

namespace Transport.Api.Adapters.Locations;

public class RegionData
{
    [JsonProperty("nom")] public string Nom { get; set; }

    [JsonProperty("code")] public string Code { get; set; }
}