using Authentication.CLI.Extensions;
using Authentication.CLI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Transport.Api.Abstractions.Interfaces.Repositories;
using Transport.Api.Adapters;
using Transport.Api.Db.Cache.Configs;
using Transport.Api.Db.Cache.Services;
using Transport.Api.Db.Repositories;

var token = "";

var host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((context, services) =>
	{
		services.AddSingleton<IFuelStationRepository, FuelStationRepository>();
		services.AddSingleton<IPriceRepository, PriceRepository>();
		services.AddSingleton<DbImportService>();

		var config = new DbCacheEndpoint();
		context.Configuration.GetSection(DbCacheEndpoint.Section).Bind(config);

		services.UseCustomAuthentication(config.AuthenticationApi);

		services.AddHttpClient<PublicFilesClient>((_, client) =>
		{
			client.BaseAddress = new Uri(config.FilesApi);
			client.DefaultRequestHeaders.Add("authentication-token", token);
		});
	})
	.Build();


var scope = host.Services.CreateScope();

var authCli = scope.ServiceProvider.GetRequiredService<AuthenticationCliService>();

token = await authCli.Login();

var importService = scope.ServiceProvider.GetRequiredService<DbImportService>();


await importService.Import();