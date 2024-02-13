using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using VendorSmart.Shared.Core.Application.Middlewares;

namespace VendorSmart.Shared.Core.DI.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication Configure(this WebApplication app, string applicationName)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI(config =>
        {
            config.DocumentTitle = applicationName;
            config.DisplayRequestDuration();
            config.DocExpansion(DocExpansion.None);
            config.EnableDeepLinking();
            config.ShowExtensions();
            config.ShowCommonExtensions();
        });


        app.UseHttpsRedirection();
        app.UseMiddleware<ExceptionHandlerMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseProblemDetails();
        app.UseHeaderPropagation();
        app.MapControllers();

        return app;
    }
}