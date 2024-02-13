using VendorSmart.Identity.Api.Endpoints.Anonymous;

namespace VendorSmart.Vendor.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void MapAuthenticatedEndpoints(this WebApplication app)
    {
        VendorAuthenticatedEndpoints.MapEndpoints(app);
    }
}