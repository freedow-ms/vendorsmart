using VendorSmart.Job.Contracts.Repositories;
using VendorSmart.Job.Entities;
using VendorSmart.Shared.Core.Domain.Contracts;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;

namespace VendorSmart.Job.Infrastructure.Contexts;

public class JobSeed
{
    public static async Task SeedJobs(IJobRepository jobRepository, ILocationRepository locationRepository, IServiceCategoryRepository serviceCategoryRepository, IDbContext dbContext)
    {
        var locationFay = await locationRepository.GetAsync("Fayette", "TX");
        var locationGla = await locationRepository.GetAsync("Glades", "FL");

        var serviceCategoryLand = await serviceCategoryRepository.GetByNameAsync("Landscaping Maintenance");
        var serviceCategoryAirCond = await serviceCategoryRepository.GetByNameAsync("Air Conditioning");

        List<JobEntity> jobs = new(){
            new JobEntity("Vendor 1",locationFay!.Id, serviceCategoryLand!.Id),
            new JobEntity("Vendor 2",locationFay!.Id, serviceCategoryAirCond!.Id),
            new JobEntity("Vendor 3",locationGla!.Id, serviceCategoryAirCond!.Id),
        };

        foreach (var job in jobs)
        {
            await jobRepository.AddAsync(job);
        };

        await dbContext.SaveChangesAsync();
    }
}