using FluentValidation;

namespace Nutrifica.Application.Users.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Id.Value)
                    .NotEmpty();
            });

        RuleFor(x => x.CurrentPassword)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(UserConstants.PasswordMinLength);
    }
}