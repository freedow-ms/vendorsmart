using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using VendorSmart.Shared.Core.DI.Extensions;
using VendorSmart.Vendor.Infrastructure.Extensions;

namespace VendorSmart.Vendor.DI.Extensions;

[ExcludeFromCodeCoverage]
public static class IServiceColletionExtensions
{
    public static IServiceCollection ConfigureVendorServices(this IServiceCollection services)
    {
        services.AddModuleApplicationServices(["VendorSmart.Vendor.Application"]);
        services.ConfigureInfrastructureServices();

        return services;
    }
}