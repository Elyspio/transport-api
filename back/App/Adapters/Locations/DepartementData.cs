namespace Transport.Api.Adapters.Locations
{
    using Newtonsoft.Json;

    public partial class DepartementData
    {
        [JsonProperty("nom")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("codeRegion")]
        public string CodeRegion { get; set; }
    }
}