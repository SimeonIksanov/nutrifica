using FluentValidation;

using Nutrifica.Application.Users;

namespace Nutrifica.Application.Authentication.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(UserConstants.UsernameMaxLength);
        RuleFor(x => x.Password)
            .NotEmpty()
            .MaximumLength(50);
    }
}
