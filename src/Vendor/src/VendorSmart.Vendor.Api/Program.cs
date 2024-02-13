using VendorSmart.Shared.Core.DI.Extensions;
using VendorSmart.Vendor.Api.Extensions;
using VendorSmart.Vendor.Application.Config;
using VendorSmart.Vendor.Application.Extensions;
using VendorSmart.Vendor.Domain.Contracts.Repositories;
using VendorSmart.Vendor.Infrastructure.Contexts;
using VendorSmart.Vendor.Infrastructure.Extensions;

var applicationName = "VendorSmart.Vendor.Api";
var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureHost(applicationName);
builder.Services.ConfigureApplicationServices(applicationName, builder.Configuration);
builder.Services.ConfigureApplicationInfrastructureServices(builder.Configuration);
builder.Services.ConfigureAppConfig<AppConfig>(builder.Configuration);

var app = builder.Build();

app.Configure(applicationName);

app.MapAuthenticatedEndpoints();

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var locationRepository = scope.ServiceProvider.GetRequiredService<ILocationRepository>();
    await Seed.SeedLocations(locationRepository);

    var serviceCategoryRepository = scope.ServiceProvider.GetRequiredService<IServiceCategoryRepository>();
    await Seed.SeedServiceCategories(serviceCategoryRepository);
}

app.Run();