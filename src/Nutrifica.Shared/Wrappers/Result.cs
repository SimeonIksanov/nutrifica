namespace Nutrifica.Shared.Wrappers;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    Error Error { get; }
}

public interface IResult<T> : IResult
{
    T Value { get; }
}
public class Result : IResult
{
    protected Result(bool success, Error error)
    {
        if (success && error != Error.None) throw new InvalidOperationException();
        if (!success && error == Error.None) throw new InvalidOperationException();
        IsSuccess = success;
        Error = error;
    }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default!, false, error);
    public static Result<TValue> Create<TValue>(TValue value) => value is null
        ? Failure<TValue>(Error.NullValue)
        : Success<TValue>(value);
}

public class Result<TValue> : Result, IResult<TValue>
{
    private readonly TValue _value;


    protected internal Result(TValue value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value => IsSuccess
        ? _value
        : throw new InvalidOperationException($"You can't access .{nameof(Value)} when .{nameof(Success)} is false");

    public static implicit operator Result<TValue>(TValue value) => Create(value);
}
