namespace Nutrifica.Shared.Wrappers;

public interface IValidationResult
{
    public static readonly Error ValidationError = new("ValidationError", "A Validation problem occured.");
    Error[] Errors { get; }
}

public sealed class ValidationResult : Result, IValidationResult
{
    public ValidationResult(Error[] errors) : base("A Validation problem occured.")
    {
        Errors = errors;
    }

    public Error[] Errors { get; }
    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    public ValidationResult(Error[] errors) : base("A Validation problem occured.")
    {
        Errors = errors;
    }

    public Error[] Errors { get; }
    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}


//15.03