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
		var log = logger.Enter(Log.Format(url));
		try
		{
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
			log.Exit();
		}
	}

	private async Task<string> GetFuelStationsXml()
	{
		var log = logger.Enter();
		try
		{
			return await Download("https://donnees.roulez-eco.fr/opendata/instantane");
		}
		finally
		{
			log.Exit();
		}
	}

	public async Task<List<FuelStationData>> GetFuelStationsByYear(int year)
	{
		var args = $"{Log.Format(year)}";

		var log = logger.Enter(args);

		try
		{
			var xml = await Download(year);
			var data = Parse(xml);
			return assembler.Convert(data);
		}
		finally
		{
			log.Exit();
		}
	}


	public FuelStations Parse(string xml)
	{
		var log = logger.Enter();
		try
		{
			var doc = new XmlDocument();
			doc.LoadXml(xml);
			var json = JsonConvert.SerializeXmlNode(doc, Formatting.None);
			doc = null;

			return FuelStations.FromJson(json);
		}
		finally
		{
			log.Exit();
		}
	}


	public async Task<List<FuelStationData>> GetFuelStations(bool refresh = false)
	{
		var log = logger.Enter(Log.Format(refresh));
		try
		{
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
			log.Exit();
		}
	}
}