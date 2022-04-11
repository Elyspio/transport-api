namespace Transport.Api.Adapters.Locations
{
    using Newtonsoft.Json;

    public class RegionData
    {
        [JsonProperty("nom")]
        public string Nom { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }
    }
}