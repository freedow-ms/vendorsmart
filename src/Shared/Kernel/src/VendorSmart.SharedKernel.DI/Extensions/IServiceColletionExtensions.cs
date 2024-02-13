using Microsoft.Extensions.DependencyInjection;
using VendorSmart.Vendor.Infrastructure.Extensions;

namespace VendorSmart.SharedKernel.DI.Extensions;

public static class IServiceColletionExtensions
{
    public static IServiceCollection ConfigureSharedServices(this IServiceCollection services)
    {
        services.ConfigureInfrastructureServices();

        return services;
    }
}