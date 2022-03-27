using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Transport.Api.Abstractions.Utils;
using Transport.Api.Adapters.FuelStation;
using Transport.Api.Web.Utils;

var frontPath = Env.Get("FRONT_PATH", "/front");

var useBuilder = () =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.ConfigureKestrel((_, options) =>
    {
        options.Listen(IPAddress.Any, 4000, listenOptions =>
        {
            // Use HTTP/3
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
        });
    });


    // Setup CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Cors", b =>
        {
            b.AllowAnyOrigin();
            b.AllowAnyHeader();
            b.AllowAnyMethod();
        });
    });


    // Inject Adapters
    builder.Services.AddHttpClient<FuelStationClient>();

    // Inject Services
    builder.Services.Scan(scan => scan
        .FromApplicationDependencies()
        .AddClasses(classes => classes.InNamespaces(
            "Core.Services",
            "Db.Repositories",
            "Db.Repositories.Internal"
        ))
        .AsImplementedInterfaces()
        .WithSingletonLifetime());

    // Setup Logging
    builder.Host.UseSerilog((_, lc) => lc
        .Enrich.FromLogContext()
        .Enrich.With(new CallerEnricher())
        .WriteTo.Console(
            LogEventLevel.Debug,
            "[{Timestamp:HH:mm:ss} {Level}{Caller}] {Message:lj}{NewLine}{Exception}",
            theme: AnsiConsoleTheme.Code
        )
    );

    // Convert Enum to String 
    builder.Services
        .AddControllers(o =>
        {
            o.Conventions.Add(new ControllerDocumentationConvention());
            o.OutputFormatters.RemoveType<StringOutputFormatter>();
        })
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
        .AddNewtonsoftJson(x => x.SerializerSettings.Converters.Add(new StringEnumConverter()));

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c => { c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}"); });

    // Setup SPA Serving
    if (builder.Environment.IsProduction())
    {
        Console.WriteLine($"Server in production, serving SPA from {frontPath} folder");
    }

    return builder;
};

var builder = useBuilder();


var app = builder.Build();

var useApp = (WebApplication application) =>
{
    application.UseSwagger();
    application.UseSwaggerUI();

    // Start Dependency Injection
    application.UseAdvancedDependencyInjection();

    // Allow CORS
    application.UseCors("Cors");

    // Setup Controllers
    application.MapControllers();

    // Start SPA serving
    if (application.Environment.IsProduction())
    {
        app.UseRouting();

        application.UseDefaultFiles(new DefaultFilesOptions
        {
            DefaultFileNames = new List<string> {"index.html"},
            RedirectToAppendTrailingSlash = true
        });
        application.UseStaticFiles();

        app.UseEndpoints(endpoints => { endpoints.MapFallbackToFile("/index.html"); });
    }

    // Start the application
    application.Run();
};

useApp(app);