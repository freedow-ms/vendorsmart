namespace VendorSmart.Shared.Core.Util.OperationResult;

public interface IResult
{
    Exception? Exception { get; }

    bool IsSuccess { get; }

    List<ResultError> Errors { get; }

    void AddError(ResultError error);

    void SetException(Exception exception);
}