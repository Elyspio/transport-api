using Abstractions.Models;
using Newtonsoft.Json;
using Serilog;
using System.IO.Compression;
using System.Text;
using System.Xml;


namespace Adapters.FuelStation;


struct FuelStationCache
{
    public List<FuelStationData> DailyData { get; set; }
    public DateTime DailyRefresh { get; set; }
    public List<FuelStationData> AllTimeData { get; set; }
}

public class FuelStationClient
{
    private readonly HttpClient client;
    private readonly ILogger logger;

    private FuelStationCache cache = new();


    public FuelStationClient(HttpClient client)
    {
        this.client = client;
        this.logger = new LoggerConfiguration().CreateLogger().ForContext<FuelStationClient>();
    }



    private async Task<string> Download(string url)
    {
        try
        {
            logger.Information($"Entering method -- Download url={url}");

            using var stream = await client.GetStreamAsync(url);

            using var archive = new ZipArchive(stream, ZipArchiveMode.Read);

            using var file = archive.Entries[0].Open();

            using var ms = new MemoryStream();

            file.CopyTo(ms);
            byte[] bytes = ms.ToArray();


            var content = Encoding.GetEncoding("ISO-8859-1").GetString(bytes);

            bytes = Array.Empty<byte>();

            return content;
        }
        finally
        {
            logger.Information($"Exiting method -- Download url={url}");
        }
    }

    private async Task<string> GetFuelStationsXml()
    {
        try
        {
            logger.Information($"Entering method -- GetFuelStationsXml");
            return await Download("https://donnees.roulez-eco.fr/opendata/instantane");

        }
        finally
        {
            logger.Information($"Exiting method -- GetFuelStationsXml");
        }
    }


    public async Task<List<FuelStationData>> GetFuelStationsAllTime()
    {
        try
        {
            logger.Information($"Entering method -- GetFuelStationsAllTime");

            if (this.cache.AllTimeData == null)
            {
                var stations = new List<FuelStationData>();
                var tasks = new List<Task>();
                var assembler = new FuelStationAssembler();

                for (int year = 2020; year < 2021; year++)
                {
                    var xml = await Download($"https://donnees.roulez-eco.fr/opendata/annee/{year}");
                    var data = Parse(xml);
                    stations.AddRange(assembler.Convert(data));

                }

                cache.AllTimeData = stations;
            }

            return cache.AllTimeData;
        }
        finally
        {
            logger.Information($"Exiting method -- GetFuelStationsAllTime");
        }
    }

    public async Task Fetch()
    {

        //for (int i = 2017; i < 2022; i++)
        {

            var xml = await Download("https://donnees.roulez-eco.fr/opendata/annee/" + 2022);
            var raw = Parse(xml);
            var data = new FuelStationAssembler().Convert(raw);

            File.WriteAllText($@"P:\own\mobile\transport-api\back\Core.Merge\2022.json", JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.None));
        }

    }

    private FuelStations Parse(string xml)
    {
        try
        {
            logger.Information($"Entering method -- Parse"); var doc = new XmlDocument();
            doc.LoadXml(xml);
            var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None);

            return FuelStations.FromJson(json);
        }
        finally
        {
            logger.Information($"Exiting method -- Parse");
        }
    }


    public async Task<List<FuelStationData>> GetFuelStations(bool refresh = false)
    {
        try
        {
            logger.Information($"Entering method -- GetFuelStations");
            if (refresh || cache.DailyRefresh + TimeSpan.FromHours(6) < DateTime.Now)
            {
                logger.Debug($"Refresh fuel stations cache at {DateTime.Now.ToShortDateString()}");
                var xml = await GetFuelStationsXml();

                cache.DailyRefresh = DateTime.Now;
                cache.DailyData = new FuelStationAssembler().Convert(Parse(xml));
            }
            return cache.DailyData;
        }

        finally
        {
            logger.Information($"Exiting method -- GetFuelStations");
        }
    }

}
