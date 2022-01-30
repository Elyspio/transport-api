using Adapters.FuelStation;
using Core.Utils;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Net;
using Web.Utils;

var frontPath = new Env().Get("FRONT_PATH", "/front");

AppContext.SetSwitch("Switch.Microsoft.AspNetCore.Mvc.EnableRangeProcessing", true);

var useBuilder = () =>
{
    var builder = WebApplication.CreateBuilder(args);
    builder.WebHost.ConfigureKestrel((_, options) =>
    {
        options.Listen(IPAddress.Any, 4000, listenOptions =>
        {
            // Use HTTP/3
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;
            listenOptions.UseHttps();
        });
    });


    // Setup CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Cors", b =>
        {
            b.AllowCredentials();
            b.WithOrigins("localhost", "127.0.0.1", "elyspio.fr");
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
        .Enrich.With(new CallerEnricher())
        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
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
        .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()))
        .AddNewtonsoftJson(x => x.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()));

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["action"]}");
        // c.CustomSchemaIds(type => type.ToString());
        //c.OperationFilter<RequireAuthAttribute.Swagger>();
    });

    // Setup SPA Serving
    if (builder.Environment.IsProduction())
        Console.WriteLine($"Server in production, serving SPA from {frontPath} folder");

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
        application.UseDefaultFiles(new DefaultFilesOptions
        {
            DefaultFileNames = new List<string> { "index.html" }
        });
        application.UseStaticFiles();
    }


    // Start the application
    application.Run();
};

useApp(app);