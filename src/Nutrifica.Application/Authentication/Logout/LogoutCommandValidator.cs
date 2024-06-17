using FluentValidation;

namespace Nutrifica.Application;

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.Jwt)
            .NotEmpty();
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
