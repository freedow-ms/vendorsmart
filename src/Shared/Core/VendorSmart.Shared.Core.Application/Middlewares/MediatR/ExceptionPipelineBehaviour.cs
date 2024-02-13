using MediatR;
using Microsoft.Extensions.Logging;
using VendorSmart.Shared.Core.Util.Exceptions;
using VendorSmart.Shared.Core.Util.OperationResult;

namespace VendorSmart.Shared.Core.Application.Middlewares.MediatR;

public class ExceptionPipelineBehaviour<TRequest, TResponse>(ILogger<ExceptionPipelineBehaviour<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult, new()
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (AppException ex)
        {
            logger.LogError(ex, "Error on exception pipeline");
            var result = new TResponse();
            result.SetException(ex);
            return result;
        }
    }
}