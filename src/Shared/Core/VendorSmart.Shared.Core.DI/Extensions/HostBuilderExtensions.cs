using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace VendorSmart.Shared.Core.DI.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigureHost(this IHostBuilder hostBuilder, string applicationName)
    {
        hostBuilder.UseSerilog((HostBuilderContext host, IServiceProvider services, LoggerConfiguration cfg) =>
        {
            cfg.ReadFrom.Configuration(host.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", applicationName)
                .Enrich.WithProperty("Environment", host.HostingEnvironment);
            cfg.MinimumLevel.Override("Microsoft", LogEventLevel.Error).MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information).MinimumLevel.Override("HealthChecks", LogEventLevel.Error).MinimumLevel.Override("MicroElements.Swashbuckle.FluentValidation", LogEventLevel.Error).MinimumLevel.Override("System.Net.Http.HttpClient.health-checks", LogEventLevel.Error);
        });
        return hostBuilder;
    }
}