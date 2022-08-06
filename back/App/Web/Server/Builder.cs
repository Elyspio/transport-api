using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Net;
using System.Text.Json.Serialization;
using Transport.Api.Abstractions.Common.Helpers;
using Transport.Api.Adapters;
using Transport.Api.Adapters.FuelStation;
using Transport.Api.Adapters.Locations;
using Transport.Api.Web.Utils;

namespace Transport.Api.Web.Server;

public class ServerBuilder
{
	private readonly string frontPath = Env.Get("FRONT_PATH", "/front");


	public ServerBuilder(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);
		builder.WebHost.ConfigureKestrel((_, options) =>
		{
			options.Listen(IPAddress.Any, 4000, listenOptions =>
			{
				// Use HTTP/3
				listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
			}
			);
		}
		);


		// Setup CORS
		builder.Services.AddCors(options =>
		{
			options.AddPolicy("Cors", b =>
			{
				b.AllowAnyOrigin();
				b.AllowAnyHeader();
				b.AllowAnyMethod();
			}
			);
		}
		);

		// Inject Adapters
		builder.Services.AddHttpClient<FuelStationClient>();
		builder.Services.AddHttpClient<LocationClient>();
		builder.Services.AddHttpClient<PublicFilesClient>(client =>
		{

		});

		// Inject Services
		builder.Services.Scan(scan => scan
			.FromApplicationDependencies()
			.AddClasses(classes => classes.InNamespaces("Transport.Api.Core.Services", "Transport.Api.Db.Repositories", "Transport.Api.Db.Repositories.Internal"))
			.AsImplementedInterfaces()
			.WithSingletonLifetime()
		);

		// Setup Logging
		builder.Host.UseSerilog((_, lc) => lc
			.Enrich.FromLogContext()
			.Enrich.With(new CallerEnricher())
			.WriteTo.Console(LogEventLevel.Debug, "[{Timestamp:HH:mm:ss} {Level}{Caller}] {Message:lj}{NewLine}{Exception}", theme: AnsiConsoleTheme.Code)
		);

		// Convert Enum to String 
		builder.Services.AddControllers(o =>
		{
			o.Conventions.Add(new ControllerDocumentationConvention());
			o.OutputFormatters.RemoveType<StringOutputFormatter>();

			o.CacheProfiles.Add("Default30", new CacheProfile
			{
				Duration = 30
			}
			);
		}
			)
			.AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
			.AddNewtonsoftJson(x => x.SerializerSettings.Converters.Add(new StringEnumConverter()));

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(options =>
		{
			if (builder.Environment.IsProduction())
				options.AddServer(new OpenApiServer
				{
					Url = "/transport"
				}
				);

			options.SwaggerDoc("v1", new OpenApiInfo { Title = "Transport API", Version = "1" });

			options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");
		}
		);

		// Setup SPA Serving
		if (builder.Environment.IsProduction()) Console.WriteLine($"Server in production, serving SPA from {frontPath} folder");

		Application = builder.Build();
	}

	public WebApplication Application { get; }
}