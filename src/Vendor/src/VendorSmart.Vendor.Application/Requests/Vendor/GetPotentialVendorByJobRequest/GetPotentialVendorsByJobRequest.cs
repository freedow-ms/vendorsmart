using MediatR;
using Microsoft.Extensions.Logging;
using VendorSmart.Shared.Core.Domain.Exceptions;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.Shared.Core.Util.OperationResult;
using VendorSmart.SharedKernel.Boundaries.Job;
using VendorSmart.Vendor.Domain.Contracts.Repositories;

namespace VendorSmart.Vendor.Application.Requests.Vendor.GetVendorsForJob;
public record GetPotentialVendorsByJobRequest(Guid JobId) : IRequest<Result<GetPotentialVendorsByJobResult>>;

public class GetPotentialVendorsByJobRequestHandler(
    IVendorRepository vendorRepository,
    IJobBoundaryApi jobBoundaryApi,
    ILogger<GetPotentialVendorsByJobRequestHandler> logger) : IRequestHandler<GetPotentialVendorsByJobRequest, Result<GetPotentialVendorsByJobResult>>
{
    public async Task<Result<GetPotentialVendorsByJobResult>> Handle(GetPotentialVendorsByJobRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var (success, job, exception) = await jobBoundaryApi.GetByIdAsync(request.JobId);

            if (!success)
                return exception!;

            if (job is null)
                return new BusinessException("JobNotFound", "Invalid job");

            var potentialVendors = await vendorRepository.GetPotentialVendors(job!.ServiceCategory, job.County, job.State);

            return new GetPotentialVendorsByJobResult(
                potentialVendors.Select(v =>
                    new GetPotentialVendorsByJobResultVendor(v.Id, v.Name, v.ServiceCategories.First(x => x.ServiceCategory.Name == job.ServiceCategory).IsCompliant)
                ).ToList());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to get potential vendors by job");
            return new AppException("GetPotentialVendorsByJobFailed", "Fail to get potential vendors by job");
        }
    }
}