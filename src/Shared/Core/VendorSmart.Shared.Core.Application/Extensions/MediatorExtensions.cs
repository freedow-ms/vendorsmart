using MediatR;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using VendorSmart.Shared.Core.Application.Exceptions;
using VendorSmart.Shared.Core.Domain.Exceptions;
using VendorSmart.Shared.Core.Util.Exceptions;

namespace VendorSmart.Shared.Core.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class MediatorExtensions
{
    public static async Task<IResult> SendCommand(this IMediator mediator, IRequest<Util.OperationResult.Result> request)
        => await mediator.Send(request) switch
        {
            (true, _, _) => Results.Ok(),
            var (_, exception, errors) => HandleError(exception!, errors),
        };

    public static async Task<IResult> SendCommand<T>(this IMediator mediator, IRequest<Util.OperationResult.Result<T>> request)
        => await mediator.Send(request) switch
        {
            (true, var result) => Results.Ok(result),
            var (_, _, exception, errors) => HandleError(exception!, errors),
        };

    private static IResult HandleError(Exception exception, List<Util.OperationResult.ResultError> errors)
    {
        var problemDetails = CreateProblemDetails(exception, errors);
        return exception switch
        {
            ValidationException e => Results.BadRequest(problemDetails! with { Type = e.Type, Title = e.Message, Status = 400 }),
            BusinessException e => Results.BadRequest(problemDetails! with { Type = e.Type, Title = e.Message, Status = 400 }),
            EntityNotFoundException e => Results.NotFound(problemDetails! with { Type = e.Type, Title = e.Message, Status = 404 }),
            AppException e => Results.Problem(title: e.Message, type: e.Type, statusCode: 500),
            _ => Results.Problem(title: "An error occurred while processing your request", detail: "An error occurred while processing your request, please try again in a few moments"),
        };
    }

    private static ProblemDetails CreateProblemDetails(Exception exception, List<Util.OperationResult.ResultError> errors)
    {
        var errorsDict = new Dictionary<string, string[]>();
        errors.ForEach(e => errorsDict.Add(JsonNamingPolicy.CamelCase.ConvertName(e.Source), [e.Description]));
        var appException = exception?.GetType() == typeof(AppException) ? (AppException)exception : null;
        var problemDetails = new ProblemDetails()
        {
            Title = appException?.Message ?? "An error occurred while processing your request",
            Errors = errorsDict,
        };
        return problemDetails;
    }
}