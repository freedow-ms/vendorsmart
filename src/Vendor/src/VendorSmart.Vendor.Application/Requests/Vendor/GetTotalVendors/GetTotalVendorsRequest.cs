using MediatR;
using Microsoft.Extensions.Logging;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.Shared.Core.Util.OperationResult;
using VendorSmart.Vendor.Domain.Contracts.Repositories;

namespace VendorSmart.Vendor.Application.Requests.Vendor.GetTotalVendors;
public record GetTotalVendorsRequest(string ServiceCategory, string County, string State) : IRequest<Result<GetTotalVendorsResult>>;

public class GetTotalVendorsRequestHandler(IVendorRepository vendorRepository, ILogger<GetTotalVendorsRequestHandler> logger) : IRequestHandler<GetTotalVendorsRequest, Result<GetTotalVendorsResult>>
{
    public async Task<Result<GetTotalVendorsResult>> Handle(GetTotalVendorsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var compliantVendors = await vendorRepository.GetPotentialVendorsCount(request.ServiceCategory, request.County, request.State, isCompliant: true, cancellationToken);
            var nonCompliantVendors = await vendorRepository.GetPotentialVendorsCount(request.ServiceCategory, request.County, request.State, isCompliant: false, cancellationToken);

            return new GetTotalVendorsResult(compliantVendors + nonCompliantVendors, compliantVendors, nonCompliantVendors);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fail to get total vendors");
            return new AppException("GetTotalVendorsFailed", "Fail to get total vendors");
        }
    }
}