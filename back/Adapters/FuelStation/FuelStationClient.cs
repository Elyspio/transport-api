using Abstraction.Models;
using Newtonsoft.Json;
using Serilog;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace Adapters.FuelStation
{
    public class FuelStationClient
    {
        private readonly HttpClient client;
        private readonly ILogger logger;
        private DateTime cacheTime;
        private List<FuelStationData> cacheData;

        public FuelStationClient(HttpClient client)
        {
            this.client = client;
            this.logger = new LoggerConfiguration().CreateLogger().ForContext<FuelStationClient>();
            GetFuelStations(true).GetAwaiter().GetResult();
        }


        private async Task<string> GetFuelStationsXml()
        {
            var url = "https://www.data.gouv.fr/fr/datasets/r/087dfcbc-8119-4814-8412-d0a387fac561";

            var stream = await client.GetStreamAsync(url);

            using var archive = new ZipArchive(stream);

            using var file = archive.Entries[0].Open();

            using var extracted = new MemoryStream();

            await file.CopyToAsync(extracted);

            return Encoding.GetEncoding("ISO-8859-1").GetString(extracted.ToArray());
        }

        public async Task<List<FuelStationData>> GetFuelStations(bool refresh = false)
        {
            if (refresh || cacheTime + TimeSpan.FromHours(6) < DateTime.Now)
            {
                logger.Debug($"Refresh fuel stations cache at {DateTime.Now.ToShortDateString()}");
                cacheTime = DateTime.Now;
                var xml = await GetFuelStationsXml();
                var doc = new XmlDocument();
                doc.LoadXml(xml);
                var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);

                var data = FuelStations.FromJson(json);

                cacheData = new FuelStationAssembler().Convert(data);
            }

            return cacheData;


        }
    }
}
