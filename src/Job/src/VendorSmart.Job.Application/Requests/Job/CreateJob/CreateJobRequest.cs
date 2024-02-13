using MediatR;
using Microsoft.Extensions.Logging;
using VendorSmart.Job.Contracts.Repositories;
using VendorSmart.Job.Entities;
using VendorSmart.Shared.Core.Domain.Exceptions;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.Shared.Core.Util.OperationResult;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;

namespace VendorSmart.Job.Application.Requests.Job.CreateJob;

public record CreateJobRequest(string Name, string ServiceCategory, CreateJobRequestLocation Location) : IRequest<Result<CreateJobResult>>;

public record CreateJobRequestLocation(string County, string State);

public class CreateJobRequestHandler(
    IJobRepository jobRepository,
    ILocationRepository locationRepository,
    IServiceCategoryRepository serviceCategoryRepository,
    ILogger<CreateJobRequestHandler> logger) : IRequestHandler<CreateJobRequest, Result<CreateJobResult>>
{
    public async Task<Result<CreateJobResult>> Handle(CreateJobRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var location = await locationRepository.GetAsync(request.Location.County, request.Location.State);

            if (location is null) return new BusinessException("InvalidLocation", "Invalid location");

            var serviceCategory = await serviceCategoryRepository.GetByNameAsync(request.ServiceCategory);
            if (serviceCategory is null)
                return new BusinessException("InvalidServiceCategory", "Invalid service category");

            var job = new JobEntity(request.Name, location.Id, serviceCategory.Id);

            await jobRepository.AddAsync(job);
            await jobRepository.SaveAsync();

            return new CreateJobResult(
                job.Id,
                job.Name,
                new(location.County, location.State),
                serviceCategory.Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail create job");
            return new AppException("CreateJobFailed", "Fail to create job");
        }
    }
}