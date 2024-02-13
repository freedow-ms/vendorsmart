using MediatR;
using VendorSmart.Job.Application.Requests.Job.CreateJob;
using VendorSmart.Shared.Core.Application.Extensions;

namespace VendorSmart.Api.Endpoints;

public static class JobEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/job", static (CreateJobRequest request, IMediator mediator)
            => mediator.SendCommand(request))
                .RequireAuthorization()
                .Produces<CreateJobResult>()
                .WithDisplayName("Create Job")
                .WithTags("Job");
    }
}