using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;
using VendorSmart.SharedKernel.Infrastructure.Contexts;
using VendorSmart.SharedKernel.Infrastructure.Repositories;

namespace VendorSmart.Vendor.Infrastructure.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class IServiceColletionExtensions
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
        {
            services.ConfigureDatabase();
            services.ConfigureRepositories();

            return services;
        }

        private static void ConfigureDatabase(this IServiceCollection services)
        {
            services.AddScoped<SharedDbContext, SharedDbContext>();
            services.AddDbContext<SharedDbContext>(options =>
                options.UseInMemoryDatabase("vendorSmart"));
        }

        private static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IServiceCategoryRepository, ServiceCategoryRepository>();
        }
    }
}