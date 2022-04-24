using System.IO.Compression;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Serilog;
using Transport.Api.Abstractions.Transports.FuelStation;
using Formatting = Newtonsoft.Json.Formatting;

namespace Transport.Api.Adapters.FuelStation;

internal struct FuelStationCache
{
    public List<FuelStationData> DailyData { get; set; }
    public DateTime DailyRefresh { get; set; }
    public List<FuelStationData> AllTimeData { get; set; }
}

public class FuelStationClient
{
    private readonly FuelStationAssembler assembler = new();
    private readonly HttpClient client;
    private readonly ILogger logger;

    private FuelStationCache cache;


    public FuelStationClient(HttpClient client)
    {
        this.client = client;
        logger = new LoggerConfiguration().CreateLogger().ForContext<FuelStationClient>();
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
            var bytes = ms.ToArray();


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
            logger.Information("Entering method -- GetFuelStationsXml");
            return await Download("https://donnees.roulez-eco.fr/opendata/instantane");
        }
        finally
        {
            logger.Information("Exiting method -- GetFuelStationsXml");
        }
    }


    public async Task<List<FuelStationData>> GetFuelStationsByYear(int year, bool useCache = true)
    {
        if (!useCache || year == 2022)
        {
            var xml = await Download($"https://donnees.roulez-eco.fr/opendata/annee/{year}");
            var data = Parse(xml);
            return assembler.Convert(data);
        }

        var cacheUrl = FuelStationsCache.Cache[year];
        var stream = await client.GetStreamAsync(cacheUrl);
        var serializer = new JsonSerializer();

        using var sr = new StreamReader(stream);
        using var jsonTextReader = new JsonTextReader(sr);
        return serializer.Deserialize<List<FuelStationData>>(jsonTextReader);
    }


    private FuelStations Parse(string xml)
    {
        try
        {
            logger.Information("Entering method -- Parse");
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            var json = JsonConvert.SerializeXmlNode(doc, Formatting.None);

            return FuelStations.FromJson(json);
        }
        finally
        {
            logger.Information("Exiting method -- Parse");
        }
    }


    public async Task<List<FuelStationData>> GetFuelStations(bool refresh = false)
    {
        try
        {
            logger.Information("Entering method -- GetFuelStations");
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
            logger.Information("Exiting method -- GetFuelStations");
        }
    }
}