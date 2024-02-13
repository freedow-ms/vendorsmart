using VendorSmart.Api.Endpoints;

namespace VendorSmart.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        VendorEndpoints.MapEndpoints(app);
        JobEndpoints.MapEndpoints(app);
    }
}