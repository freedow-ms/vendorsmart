using VendorSmart.Shared.Core.Util.OperationResult;
using VendorSmart.SharedKernel.Boundaries.Job.Models;

namespace VendorSmart.SharedKernel.Boundaries.Job;

public interface IJobBoundaryApi
{
    Task<Result<GetJobPublicModel?>> GetByIdAsync(Guid jobId);
}

