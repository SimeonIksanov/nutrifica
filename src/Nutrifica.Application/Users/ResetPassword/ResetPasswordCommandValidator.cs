using FluentValidation;

namespace Nutrifica.Application.Users.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    private const int PasswordMinLength = 6;
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.Id.Value)
                .NotEmpty());

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(PasswordMinLength);
    }
}