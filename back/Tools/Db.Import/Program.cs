using Abstractions.Interfaces.Repositories;
using Abstractions.Models;
using Db.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<IFuelStationRepository, FuelStationRepository>();
    })
    .Build();


var scope = host.Services.CreateScope();

var repo = scope.ServiceProvider.GetRequiredService<IFuelStationRepository>();



Console.WriteLine("Database Import: starting");


await repo.Clear();
Console.WriteLine("Database Import: database cleared");




FuelStationData[] Parse()
{
    using var stream = File.OpenText("merged.json");

    var serializer = new JsonSerializer();




    using (var jsonTextReader = new JsonTextReader(stream))
    {
        return serializer.Deserialize<FuelStationData[]>(jsonTextReader);
    }
}

var data = Parse();


Task Add(FuelStationData station)
{
    return repo.Add(station.Id, station.Location, station.Prices, station.Services);
}


Task.WaitAll(data.Select(station => Add(station)).ToArray());

Console.WriteLine($"Database Import: {data.Length} stations inserted");





