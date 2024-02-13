using MediatR;
using Microsoft.Extensions.Logging;
using VendorSmart.Shared.Core.Domain.Exceptions;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.Shared.Core.Util.OperationResult;
using VendorSmart.SharedKernel.Domain.Contracts.Repositories;
using VendorSmart.SharedKernel.Domain.Entities;
using VendorSmart.Vendor.Domain.Contracts.Repositories;
using VendorSmart.Vendor.Domain.Entities;

namespace VendorSmart.Vendor.Application.Requests.Vendor.CreateVendor;

public record CreateVendorRequest(string Name, List<CreateVendorRequestServiceCategory> ServiceCategories, CreateVendorRequestLocation Location) : IRequest<Result<CreateVendorResult>>;

public record CreateVendorRequestLocation(string County, string State);

public record CreateVendorRequestServiceCategory(string Name, bool IsCompliant);


public class CreateVendorRequestHandler(
    IVendorRepository vendorRepository,
    ILocationRepository locationRepository,
    IServiceCategoryRepository serviceCategoryRepository,
    ILogger<CreateVendorRequestHandler> logger) : IRequestHandler<CreateVendorRequest, Result<CreateVendorResult>>
{
    public async Task<Result<CreateVendorResult>> Handle(CreateVendorRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var location = await locationRepository.GetAsync(request.Location.County, request.Location.State);

            if (location is null) return new BusinessException("InvalidLocation", "Invalid location");

            var vendor = new VendorEntity(request.Name, location.Id);

            var serviceCategories = new List<ServiceCategoryEntity>();
            foreach (var sc in request.ServiceCategories)
            {
                var serviceCategory = await serviceCategoryRepository.GetByNameAsync(sc.Name);
                if (serviceCategory is null)
                    return new BusinessException("InvalidServiceCategory", "Invalid service category");

                serviceCategories.Add(serviceCategory);
                vendor.AddServiceCategory(serviceCategory.Id, sc.IsCompliant);
            }

            await vendorRepository.AddAsync(vendor);
            await vendorRepository.SaveAsync();

            return new CreateVendorResult(
                vendor.Id,
                vendor.Name,
                new(location.County, location.State),
                vendor.ServiceCategories.Select(sc =>
                    new CreateVendorResultServiceCategory(serviceCategories.First(x => x.Id == sc.ServiceCategoryId).Name, sc.IsCompliant))
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to create vendor");
            return new AppException("CreateVendorFailed", "Fail to create vendor");
        }
    }
}