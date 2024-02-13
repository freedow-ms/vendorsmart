using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using VendorSmart.Job.Contracts.Repositories;
using VendorSmart.Job.Infrastructure.Contexts;
using VendorSmart.Job.Infrastructure.Repositories;

namespace VendorSmart.Job.Infrastructure.Extensions
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
            services.AddScoped<JobDbContext, JobDbContext>();
            services.AddDbContext<JobDbContext>(options =>
                options.UseInMemoryDatabase("vendorSmart"));
        }

        private static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IJobRepository, JobRepository>();
        }
    }
}