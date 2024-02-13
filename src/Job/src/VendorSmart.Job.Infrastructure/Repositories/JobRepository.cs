using Microsoft.EntityFrameworkCore;
using VendorSmart.Job.Contracts.Repositories;
using VendorSmart.Job.Entities;
using VendorSmart.Job.Infrastructure.Contexts;

namespace VendorSmart.Job.Infrastructure.Repositories;

public class JobRepository(JobDbContext context) : IJobRepository
{
    public async Task AddAsync(JobEntity vendor) => await context.Jobs.AddAsync(vendor);

    public async Task<JobEntity?> GetByIdAsync(Guid id) => await context.Jobs.Include(x => x.Location).Include(x => x.ServiceCategory).FirstOrDefaultAsync(x => x.Id == id);

    public Task SaveAsync() => context.SaveChangesAsync();
}
