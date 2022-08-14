using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Text;
using System.Xml;
using Transport.Api.Abstractions.Common.Helpers;
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
	private readonly FuelStationApiAssembler assembler = new();
	private readonly HttpClient client;
	private readonly ILogger<FuelStationClient> logger;

	private FuelStationCache cache;


	public FuelStationClient(HttpClient client, ILogger<FuelStationClient> logger)
	{
		this.client = client;
		this.logger = logger;
	}


	public async Task<string> Download(int year)
	{
		var url = $"https://donnees.roulez-eco.fr/opendata/annee/{year}";
		return await Download(url);
	}

	public async Task<string> Download(string url)
	{
		try
		{
			logger.Enter(Log.Format(url));


			using var client = new HttpClient();

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
			logger.Exit(Log.Format(url));
		}
	}

	private async Task<string> GetFuelStationsXml()
	{
		try
		{
			logger.Enter();
			return await Download("https://donnees.roulez-eco.fr/opendata/instantane");
		}
		finally
		{
			logger.Exit();
		}
	}

	public async Task<List<FuelStationData>> GetFuelStationsByYear(int year, bool useCache = true)
	{
		var args = $"{Log.Format(year)} {Log.Format(useCache)}";

		logger.Enter(args);

		try
		{
			if (!useCache || year == 2022)
			{
				var xml = await Download(year);
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
		finally
		{
			logger.Exit(args);
		}
	}


	public FuelStations Parse(string xml)
	{
		try
		{
			logger.Enter();
			var doc = new XmlDocument();
			doc.LoadXml(xml);
			var json = JsonConvert.SerializeXmlNode(doc, Formatting.None);
			doc = null;

			return FuelStations.FromJson(json);
		}
		finally
		{
			logger.Exit();
		}
	}


	public async Task<List<FuelStationData>> GetFuelStations(bool refresh = false)
	{
		try
		{
			logger.Enter(Log.Format(refresh));
			if (refresh || cache.DailyRefresh + TimeSpan.FromHours(6) < DateTime.Now)
			{
				logger.LogDebug($"Refresh fuel stations cache at {DateTime.Now.ToShortDateString()}");
				var xml = await GetFuelStationsXml();

				cache.DailyRefresh = DateTime.Now;
				cache.DailyData = new FuelStationApiAssembler().Convert(Parse(xml));
			}

			return cache.DailyData;
		}
		finally
		{
			logger.Exit(Log.Format(refresh));
		}
	}
}