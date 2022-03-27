using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console;
using Transport.Api.Adapters.FuelStation;
using Transport.Api.Db.Repositories;
using Transport.Api.Db.Update.Services;

AnsiConsole.Write(
    new FigletText("Database Updater")
        .LeftAligned()
        .Color(Color.SeaGreen1_1));


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(service =>
    {
        service.AddHttpClient<FuelStationClient>();
        service.AddSingleton<PriceUpdateService>();
        service.AddSingleton<FuelStationRepository>();
        service.AddSingleton<PriceRepository>();
    })
    .Build();


var scope = host.Services.CreateScope();

var service = scope.ServiceProvider.GetRequiredService<PriceUpdateService>();

Console.WriteLine("");

var years = Enumerable.Range(2007, DateTime.Now.Year - 2007 + 1);


var reset = AnsiConsole.Confirm("Reset all?", false);

if (reset)
{
    await service.ClearAll();


    foreach (var year in years)
    {
        await service.UpdatePrices(year);
    }
}
else
{
    var year = AnsiConsole.Prompt(
        new SelectionPrompt<int>()
            .Title("Which [blue]year[/] to refresh?")
            .PageSize(5)
            .AddChoices(years.Reverse().ToArray()));

    await service.UpdatePrices(year);
}


//var year = 2021;