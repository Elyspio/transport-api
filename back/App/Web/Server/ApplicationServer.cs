namespace Transport.Api.Web.Server;

public static class ApplicationServer
{
    public static WebApplication Initialize(this WebApplication application)
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
            application.UseRouting();

            application.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new List<string> { "index.html" },
                RedirectToAppendTrailingSlash = true
            }
            );
            application.UseStaticFiles();

            application.UseEndpoints(endpoints => { endpoints.MapFallbackToFile("/index.html"); });
        }
        return application;
    }
}