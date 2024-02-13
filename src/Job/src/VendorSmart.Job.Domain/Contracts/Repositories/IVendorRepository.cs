using VendorSmart.Job.Entities;

namespace VendorSmart.Job.Contracts.Repositories;

public interface IJobRepository
{
    Task<JobEntity?> GetByIdAsync(Guid id);

    Task AddAsync(JobEntity vendor);

    Task SaveAsync();
}
