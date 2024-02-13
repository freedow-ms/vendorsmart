using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using VendorSmart.Identity.Infrastructure.Repositories;
using VendorSmart.Vendor.Domain.Contracts.Repositories;
using VendorSmart.Vendor.Infrastructure.Contexts;

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
            services.AddScoped<VendorDbContext, VendorDbContext>();
            services.AddDbContext<VendorDbContext>(options =>
                options.UseInMemoryDatabase("vendorSmart"));
        }

        private static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IVendorRepository, VendorRepository>();
        }
    }
}