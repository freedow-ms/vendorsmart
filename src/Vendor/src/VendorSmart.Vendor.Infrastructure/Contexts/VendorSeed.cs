using VendorSmart.Shared.Core.Domain.Contracts;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;
using VendorSmart.Vendor.Domain.Contracts.Repositories;
using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Infrastructure.Contexts;

public class VendorSeed
{
    public static async Task SeedVendors(IVendorRepository vendorRepository, ILocationRepository locationRepository, IServiceCategoryRepository serviceCategoryRepository, IDbContext dbContext)
    {
        var locationFay = await locationRepository.GetAsync("Fayette", "TX");
        var locationGla = await locationRepository.GetAsync("Glades", "FL");

        var serviceCategoryLand = await serviceCategoryRepository.GetByNameAsync("Landscaping Maintenance");
        var serviceCategoryAirCond = await serviceCategoryRepository.GetByNameAsync("Air Conditioning");

        var vendor1 = new VendorEntity("Vendor 1", locationFay!.Id);
        vendor1.AddServiceCategory(serviceCategoryLand!.Id, false);
        vendor1.AddServiceCategory(serviceCategoryAirCond!.Id, false);

        var vendor2 = new VendorEntity("Vendor 2", locationFay!.Id);
        vendor2.AddServiceCategory(serviceCategoryAirCond!.Id, false);

        var vendor3 = new VendorEntity("Vendor 3", locationFay!.Id);
        vendor3.AddServiceCategory(serviceCategoryAirCond!.Id, true);

        var vendor4 = new VendorEntity("Vendor 4", locationGla!.Id);
        vendor4.AddServiceCategory(serviceCategoryLand!.Id, true);

        var vendor5 = new VendorEntity("Vendor 5", locationGla!.Id);
        vendor5.AddServiceCategory(serviceCategoryAirCond!.Id, false);

        var vendor6 = new VendorEntity("Vendor 6", locationGla!.Id);
        vendor6.AddServiceCategory(serviceCategoryAirCond!.Id, true);

        List<VendorEntity> vendors = new() { vendor1, vendor2, vendor3, vendor4, vendor5, vendor6 };

        foreach (var vendor in vendors)
        {
            await vendorRepository.AddAsync(vendor);
        };

        await dbContext.SaveChangesAsync();
    }
}