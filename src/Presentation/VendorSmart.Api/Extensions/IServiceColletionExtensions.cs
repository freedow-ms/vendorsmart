using System.Diagnostics.CodeAnalysis;
using VendorSmart.Shared.Core.DI.Extensions;

namespace VendorSmart.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class IServiceColletionExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureBaseServices();
        services.ConfigureAuth(configuration);
    }
}