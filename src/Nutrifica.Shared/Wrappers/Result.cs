namespace Nutrifica.Shared.Wrappers;

public interface IResult
{
    bool Success { get; }
    bool Failure { get; }
    string Message { get; }
}

public interface IResult<T> : IResult
{
    T Value { get; }
}
public class Result : IResult
{
    public string Message { get; } = null!;

    public bool Success => string.IsNullOrEmpty(Message);
    public bool Failure => !Success;

    protected Result() { }

    protected Result(string message)
    {
        if (String.IsNullOrEmpty(message))
            throw new ArgumentException();

        Message = message;
    }

    public static Result Fail(string message) { return new Result(message); }
    public static Result Ok() { return new Result(); }
}

public class Result<T> : Result, IResult<T>
{
    private readonly T _value;
    
    public new static Result<T> Fail(string message) => new(message);
    public static Result<T> Ok(T value) => new(value);

    protected Result(T value)
    {
        _value = value;
    }
    
    protected Result(string message) : base(message)
    {
        _value = default!;
    }

    public T Value => Success
        ? _value 
        : throw new InvalidOperationException($"You can't access .{nameof(Value)} when .{nameof(Success)} is false");
}
