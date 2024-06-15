using FluentValidation;

namespace Nutrifica.Application.Accounts.UserCreate;

public class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(30);
    }
}
