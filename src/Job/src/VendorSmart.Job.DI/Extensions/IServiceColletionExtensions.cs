using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using VendorSmart.Job.Application.BoundaryApi;
using VendorSmart.Job.Infrastructure.Extensions;
using VendorSmart.Shared.Core.DI.Extensions;
using VendorSmart.SharedKernel.Boundaries.Job;

namespace VendorSmart.Job.DI.Extensions;

[ExcludeFromCodeCoverage]
public static class IServiceColletionExtensions
{
    public static IServiceCollection ConfigureJobServices(this IServiceCollection services)
    {
        services.AddModuleApplicationServices(["VendorSmart.Job.Application"]);
        services.ConfigureInfrastructureServices();
        services.AddScoped<IJobBoundaryApi, JobBoundaryApi>();

        return services;
    }
}