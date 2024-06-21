using FluentValidation;

namespace Nutrifica.Application.Users.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.Id.Value)
                .NotEmpty());

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(UserConstants.PasswordMinLength);
    }
}