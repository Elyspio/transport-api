using System.Collections.Concurrent;
using System.IO.Compression;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Serilog;
using Transport.Api.Abstractions.Transports;
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


    public async Task<List<FuelStationData>> GetFuelStationsByYear(int year)
    {
        var xml = await Download($"https://donnees.roulez-eco.fr/opendata/annee/{year}");
        var data = Parse(xml);
        return assembler.Convert(data);
    }


    public async Task<List<FuelStationData>> GetFuelStationsAllTime()
    {
        try
        {
            logger.Information("Entering method -- GetFuelStationsAllTime");

            if (cache.AllTimeData == null)
            {
                var stations = new ConcurrentBag<FuelStationData>();
                var years = new List<int> {2020, 2021};
                var tasks = new List<Task>();

                await Parallel.ForEachAsync(years, async (year, cancel) => {
                        var data = await GetFuelStationsByYear(year);
                        data.ForEach(station => stations.Add(station));
                    }
                );


                cache.AllTimeData = stations.ToList();
            }

            return cache.AllTimeData;
        }
        finally
        {
            logger.Information("Exiting method -- GetFuelStationsAllTime");
        }
    }

    public async Task Fetch()
    {
        //for (int i = 2017; i < 2022; i++)
        {
            var xml = await Download("https://donnees.roulez-eco.fr/opendata/annee/" + 2022);
            var raw = Parse(xml);
            var data = new FuelStationAssembler().Convert(raw);

            File.WriteAllText(@"P:\own\mobile\transport-api\back\Core.Merge\2022.json", JsonConvert.SerializeObject(data, Formatting.None));
        }
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