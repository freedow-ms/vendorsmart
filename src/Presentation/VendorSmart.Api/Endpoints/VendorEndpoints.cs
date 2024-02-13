using MediatR;
using VendorSmart.Shared.Core.Application.Extensions;
using VendorSmart.Vendor.Application.Requests.Vendor.CreateVendor;
using VendorSmart.Vendor.Application.Requests.Vendor.GetTotalVendors;
using VendorSmart.Vendor.Application.Requests.Vendor.GetVendorsForJob;

namespace VendorSmart.Api.Endpoints;

public static class VendorEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/vendor", static (CreateVendorRequest request, IMediator mediator)
            => mediator.SendCommand(request))
                .RequireAuthorization()
                .Produces<CreateVendorResult>()
                .WithDisplayName("Create Vendor")
                .WithTags("Vendor");

        app.MapGet("/vendor/potential", static (string serviceCategory, string county, string state, IMediator mediator)
            => mediator.SendCommand(new GetPotentialVendorsRequest(serviceCategory, county, state)))
                .RequireAuthorization()
                .Produces<GetPotentialVendorsResult>()
                .WithDisplayName("Get Potential Vendors")
                .WithTags("Vendor");

        app.MapGet("/vendor/potential/job/{jobId}", static (Guid jobId, IMediator mediator)
            => mediator.SendCommand(new GetPotentialVendorsByJobRequest(jobId)))
                .RequireAuthorization()
                .Produces<GetPotentialVendorsResult>()
                .WithDisplayName("Get Potential Vendors by Job")
                .WithTags("Vendor");

        app.MapGet("/vendor/total", static (string serviceCategory, string county, string state, IMediator mediator)
            => mediator.SendCommand(new GetTotalVendorsRequest(serviceCategory, county, state)))
                .Produces<GetTotalVendorsResult>()
                .WithDisplayName("Get Total Vendors")
                .WithTags("Vendor");
    }
}