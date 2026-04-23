using DevBoard.SharedKernel.Errors;

namespace DevBoard.SharedKernel.Results;

public class Result
{
    protected Result(bool isSuccess, DomainError? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public DomainError? Error { get; }

    public static Result Success() => new(true, null);

    public static Result Failure(DomainError error) => new(false, error);
}
