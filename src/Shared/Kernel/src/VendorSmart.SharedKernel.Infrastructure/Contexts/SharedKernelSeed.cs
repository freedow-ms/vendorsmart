using VendorSmart.Shared.Core.Domain.Contracts;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;
using VendorSmart.SharedKernel.Domain.Entities;

namespace VendorSmart.Vendor.Infrastructure.Contexts;

public class SharedKernelSeed
{
    public static async Task SeedLocations(ILocationRepository locationRepository, IDbContext dbContext)
    {
        List<LocationEntity> locations = new(){
            new LocationEntity("Glades", "FL"),
            new LocationEntity("Gulf", "FL"),
            new LocationEntity("Hamilton", "FL"),
            new LocationEntity("Hardee", "FL"),
            new LocationEntity("Hendry", "FL"),
            new LocationEntity("El Paso", "TX"),
            new LocationEntity("Erath", "TX"),
            new LocationEntity("Falls", "TX"),
            new LocationEntity("Fannin", "TX"),
            new LocationEntity("Fayette", "TX"),
            new LocationEntity("Fisher", "TX"),
        };

        foreach (var location in locations)
        {
            await locationRepository.AddAsync(location);
        };

        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedServiceCategories(IServiceCategoryRepository serviceCategoryRepository, IDbContext dbContext)
    {
        List<ServiceCategoryEntity> serviceCategories = new(){
            new ServiceCategoryEntity("Access Control Software"),
            new ServiceCategoryEntity("Air Conditioning"),
            new ServiceCategoryEntity("Landscaping"),
            new ServiceCategoryEntity("Landscaping Maintenance"),
            new ServiceCategoryEntity("Snow and Ice Removal"),
            new ServiceCategoryEntity("Sewer and Water Pipelining"),
        };

        foreach (var serviceCategory in serviceCategories)
        {
            await serviceCategoryRepository.AddAsync(serviceCategory);
        };

        await dbContext.SaveChangesAsync();
    }
}