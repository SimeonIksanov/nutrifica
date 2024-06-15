using FluentValidation;

namespace Nutrifica.Application;

public class RefreshTokensCommandValidator : AbstractValidator<RefreshTokensCommand>
{
    public RefreshTokensCommandValidator()
    {
        RuleFor(x => x.jwt)
            .NotEmpty();

        RuleFor(x => x.refreshToken)
            .NotEmpty();
    }
}
