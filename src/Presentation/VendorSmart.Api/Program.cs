using VendorSmart.Api.Extensions;
using VendorSmart.Job.Contracts.Repositories;
using VendorSmart.Job.DI.Extensions;
using VendorSmart.Job.Infrastructure.Contexts;
using VendorSmart.Shared.Core.DI.Extensions;
using VendorSmart.SharedKernel.DI.Extensions;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;
using VendorSmart.SharedKernel.Infrastructure.Contexts;
using VendorSmart.Vendor.DI.Extensions;
using VendorSmart.Vendor.Domain.Contracts.Repositories;
using VendorSmart.Vendor.Infrastructure.Contexts;

var applicationName = "VendorSmart.Api";
var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureHost(applicationName);

builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureSharedServices();
builder.Services.ConfigureVendorServices();
builder.Services.ConfigureJobServices();

var app = builder.Build();

app.Configure(applicationName);

app.MapEndpoints();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var sharedContext = scope.ServiceProvider.GetRequiredService<SharedDbContext>();
    var locationRepository = scope.ServiceProvider.GetRequiredService<ILocationRepository>();
    await SharedKernelSeed.SeedLocations(locationRepository, sharedContext);

    var serviceCategoryRepository = scope.ServiceProvider.GetRequiredService<IServiceCategoryRepository>();
    await SharedKernelSeed.SeedServiceCategories(serviceCategoryRepository, sharedContext);

    var vendorContext = scope.ServiceProvider.GetRequiredService<VendorDbContext>();
    var vendorRepository = scope.ServiceProvider.GetRequiredService<IVendorRepository>();
    await VendorSeed.SeedVendors(vendorRepository, locationRepository, serviceCategoryRepository, vendorContext);

    var jobContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();
    var jobRepository = scope.ServiceProvider.GetRequiredService<IJobRepository>();
    await JobSeed.SeedJobs(jobRepository, locationRepository, serviceCategoryRepository, jobContext);
}

app.Run();