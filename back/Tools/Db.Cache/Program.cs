// See https://aka.ms/new-console-template for more information
using Authentication.CLI.Extensions;
using Authentication.CLI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Spectre.Console;
using Transport.Api.Adapters;
using Transport.Api.Adapters.FuelStation;
using Transport.Api.Db.Cache.Configs;
using Transport.Api.Db.Cache.Services;
using Transport.Api.Db.Repositories;

AnsiConsole.Write(new FigletText("Database Cache").LeftAligned().Color(Color.SeaGreen1_1));



var token = "";

var host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((context, service) =>
	{

		var config = new DbCacheEndpoint();
		context.Configuration.GetSection(DbCacheEndpoint.Section).Bind(config);


		service.UseCustomAuthentication(config.AuthenticationApi);

		service.AddSingleton<PriceCacheService>();
		service.AddSingleton<FuelStationRepository>();
		service.AddSingleton<PriceRepository>();
		service.AddHttpClient<FuelStationClient>();
		service.AddHttpClient<PublicFilesClient>((_, client) =>
		{
			client.BaseAddress = new Uri(config.FilesApi);
			client.DefaultRequestHeaders.Add("authentication-token", token);
		});
	})
	.ConfigureLogging((ctx, logging) =>
	{

		logging.ClearProviders();
		logging.AddSimpleConsole(options =>
		{
			options.SingleLine = true;
			options.ColorBehavior = Microsoft.Extensions.Logging.Console.LoggerColorBehavior.Enabled;
		});
	})
	.Build();


var scope = host.Services.CreateScope();

var authenticationCliService = scope.ServiceProvider.GetRequiredService<AuthenticationCliService>();

AnsiConsole.MarkupLine("This tool requires auth");
token = await authenticationCliService.Login();

var service = scope.ServiceProvider.GetRequiredService<PriceCacheService>();


var years = Enumerable.Range(2007, DateTime.Now.Year - 2007).ToList();


Console.WriteLine($"Years to put in cache: {string.Join(", ", years)}");


var file = await service.Create(years);


var cacheFile = @"P:\own\mobile\transport-api\back\Tools\Db.Cache\cache.json";
File.WriteAllText(cacheFile, JsonConvert.SerializeObject(file, Formatting.Indented));

