using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Transport.Api.Abstractions.Enums;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Abstractions.Transports.FuelStation;
using Transport.Api.Db.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((_, services) =>
	{
		services.AddSingleton<IFuelStationRepository, FuelStationRepository>();
		services.AddSingleton<IPriceRepository, PriceRepository>();
	})
	.Build();


var scope = host.Services.CreateScope();

var repoStations = scope.ServiceProvider.GetRequiredService<IFuelStationRepository>();
var repoPrices = scope.ServiceProvider.GetRequiredService<IPriceRepository>();

Console.WriteLine("Database Import: starting");

await repoStations.Clear();
await repoPrices.Clear();

Console.WriteLine("Database Import: database cleared");

FuelStationData[] Parse()
{
	using var stream = File.OpenText("merged.json");

	var serializer = new JsonSerializer();

	using var jsonTextReader = new JsonTextReader(stream);

	return serializer.Deserialize<FuelStationData[]>(jsonTextReader);
}

var data = Parse();


async Task Add(FuelStationData station)
{
	var tasks = new List<Task>();
	foreach (Fuel fuel in Enum.GetValues(typeof(Fuel)))
	{
		foreach (var price in station.Prices[fuel])
		{
			tasks.Add(repoPrices.Add(station.Id, fuel, price.Date, price.Value));
		}
	}

	tasks.Add(repoStations.Add(station.Id, station.Location, station.Services));

	await Task.WhenAll(tasks.ToArray());
}


Task.WaitAll(data.Select(station => Add(station)).ToArray());

Console.WriteLine($"Database Import: {data.Length} stations inserted");





