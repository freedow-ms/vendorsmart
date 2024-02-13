using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using VendorSmart.Shared.Core.Application.Middlewares;
using VendorSmart.Shared.Core.Application.Middlewares.MediatR;
using VendorSmart.Shared.Core.Util.Specification.Evaluator;

namespace VendorSmart.Shared.Core.DI.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection ConfigureBaseServices(this IServiceCollection services)
    {
        services.AddProblemDetails(ConfigureProblemDetails);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddFluentValidationRulesToSwagger();
        services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy(), ["api"]);
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "VendorSmart API", Version = "v1" });

            c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {{
                new OpenApiSecurityScheme { Reference = new OpenApiReference{ Type = ReferenceType.SecurityScheme, Id = "basic" } },
                Array.Empty<string>()
            }});
        });
        services.AddHttpContextAccessor();
        services.AddHeaderPropagation();
        services.AddSingleton<ISpecificationEvaluator, SpecificationEvaluator>();

        return services;
    }

    public static void ConfigureAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
    }

    public static void AddModuleApplicationServices(this IServiceCollection services, string[] serviceAssemblies)
    {
        var assemblies = serviceAssemblies.Select(Assembly.Load).ToArray();
        services.AddValidatorsFromAssemblies(assemblies);
        services.Configure<JsonOptions>(options => options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
        AddMediatr(services, assemblies);
    }

    private static void ConfigureProblemDetails(Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options)
    {
        options.MapToStatusCode<BadHttpRequestException>(StatusCodes.Status400BadRequest);
        options.MapToStatusCode<BadHttpRequestException>(StatusCodes.Status422UnprocessableEntity);
        options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
    }

    private static void AddMediatr(IServiceCollection services, Assembly[] assemblies)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(assemblies)
            .AddOpenBehavior(typeof(ValidationPipelineBehavior<,>))
            .AddOpenBehavior(typeof(ExceptionPipelineBehaviour<,>)));
    }
}