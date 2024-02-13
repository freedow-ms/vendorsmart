using VendorSmart.Job.Contracts.Repositories;
using VendorSmart.Shared.Core.Util.OperationResult;
using VendorSmart.SharedKernel.Boundaries.Job;
using VendorSmart.SharedKernel.Boundaries.Job.Models;

namespace VendorSmart.Job.Application.BoundaryApi;
public class JobBoundaryApi(IJobRepository jobRepository) : IJobBoundaryApi
{
    public async Task<Result<GetJobPublicModel?>> GetByIdAsync(Guid jobId)
    {
        var job = await jobRepository.GetByIdAsync(jobId);
        return job is not null ?
            new GetJobPublicModel(job.Id, job.Location.County, job.Location.State, job.ServiceCategory.Name) :
            null;
    }
}
