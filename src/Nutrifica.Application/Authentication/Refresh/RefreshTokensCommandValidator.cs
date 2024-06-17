using FluentValidation;

namespace Nutrifica.Application;

public class RefreshTokensCommandValidator : AbstractValidator<RefreshTokensCommand>
{
    public RefreshTokensCommandValidator()
    {
        RuleFor(x => x.Jwt)
            .NotEmpty();

        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
