using MediatR;
using Microsoft.Extensions.Logging;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.Shared.Core.Util.OperationResult;
using VendorSmart.Vendor.Domain.Contracts.Repositories;

namespace VendorSmart.Vendor.Application.Requests.Vendor.GetVendorsForJob;
public record GetPotentialVendorsRequest(string ServiceCategory, string County, string State) : IRequest<Result<GetPotentialVendorsResult>>;

public class GetPotentialVendorsRequestHandler(IVendorRepository vendorRepository, ILogger<GetPotentialVendorsRequestHandler> logger) : IRequestHandler<GetPotentialVendorsRequest, Result<GetPotentialVendorsResult>>
{
    public async Task<Result<GetPotentialVendorsResult>> Handle(GetPotentialVendorsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var potentialVendors = await vendorRepository.GetPotentialVendors(request.ServiceCategory, request.County, request.State);

            return new GetPotentialVendorsResult(
                potentialVendors.Select(v =>
                    new GetPotentialVendorsResultVendor(v.Id, v.Name, v.ServiceCategories.First(x => x.ServiceCategory.Name == request.ServiceCategory).IsCompliant)
                ).ToList());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to get potential vendors");
            return new AppException("GetPotentialVendorsFailed", "Fail to get potential vendors");
        }
    }
}