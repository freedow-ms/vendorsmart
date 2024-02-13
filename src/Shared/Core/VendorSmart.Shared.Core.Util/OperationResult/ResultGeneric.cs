namespace VendorSmart.Shared.Core.Util.OperationResult;

public struct Result<T> : IResult
{
    public T? Value { get; }

    public List<ResultError> Errors { get; private set; } = [];

    public Exception? Exception { get; private set; }

    public bool IsSuccess { get; }

    public Result(T value)
    {
        IsSuccess = true;
        Exception = null;
        Value = value;
        Errors = [];
    }

    public Result(Exception exception, ResultError? error = null)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        IsSuccess = false;
        Value = default;
        Errors = error is null ? new() : new() { error };
    }

    public void AddError(ResultError error)
    {
        Errors ??= [];
        Errors.Add(error);
        Exception ??= new Exception();
    }

    public void SetException(Exception exception) => Exception = exception;

    public static implicit operator Result<T>(T value)
        => new(value);

    public static implicit operator Result<T>(Exception exception)
        => new(exception);

    public readonly void Deconstruct(out bool success, out T? value)
        => (success, value) = (IsSuccess, Value);

    public readonly void Deconstruct(out bool success, out T? value, out Exception? exception, out List<ResultError> errors)
        => (success, value, exception, errors) = (IsSuccess, Value, Exception, Errors);

    public readonly void Deconstruct(out bool success, out T? value, out Exception? exception)
        => (success, value, exception) = (IsSuccess, Value, Exception);

    public static implicit operator bool(Result<T> result)
        => result.IsSuccess;
}