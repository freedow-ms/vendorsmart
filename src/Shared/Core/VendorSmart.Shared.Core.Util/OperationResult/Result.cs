namespace VendorSmart.Shared.Core.Util.OperationResult;

public struct Result : IResult
{
    public Exception? Exception { get; private set; }

    public List<ResultError> Errors { get; private set; } = [];

    public bool IsSuccess { get; }

    public Result(bool success)
    {
        IsSuccess = success;
        Exception = null;
        Errors = [];
    }

    public Result(Exception exception, ResultError? error = null)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        IsSuccess = false;
        Errors = error is null ? new() : new() { error };
    }

    public void AddError(ResultError error)
    {
        Errors ??= [];
        Errors.Add(error);
        Exception ??= new Exception();
    }
    public void SetException(Exception exception) => Exception = exception;

    public static Result Success()
        => new(true);

    public static Result Error(Exception exception, ResultError? error = null)
        => new(exception, error);

    public static Result<T> Success<T>(T value)
    => new(value);

    public static Result<T> Error<T>(Exception exception, ResultError? error = null)
        => new(exception, error);

    public static implicit operator Result(Exception exception)
        => new(exception);

    public readonly void Deconstruct(out bool success, out Exception? exception)
    => (success, exception) = (IsSuccess, Exception);

    public readonly void Deconstruct(out bool success, out Exception? exception, out List<ResultError> errors)
        => (success, exception, errors) = (IsSuccess, Exception, Errors);
}