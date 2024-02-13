using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VendorSmart.Shared.Core.Util.OperationResult;
using ValidationException = VendorSmart.Shared.Core.Application.Exceptions.ValidationException;

namespace VendorSmart.Shared.Core.Application.Middlewares.MediatR;

public class ValidationPipelineBehavior<TRequest, TResponse>(IServiceProvider serviceProvider)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : IResult, new()
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var result = new TResponse();
            result.SetException(new ValidationException());

            foreach (var error in validationResult.Errors)
            {
                result.AddError(new(error.PropertyName, error.ErrorMessage));
            }

            return result;
        }

        return await next();
    }

    private async Task<ValidationResult> ValidateAsync(TRequest request)
    {
        using var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetService<IValidator<TRequest>>();
        return await (service == null ? Task.FromResult(new ValidationResult()) : service.ValidateAsync(request));
    }
}