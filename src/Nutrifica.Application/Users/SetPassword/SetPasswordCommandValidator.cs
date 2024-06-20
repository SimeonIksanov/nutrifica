using FluentValidation;

namespace Nutrifica.Application.Users.SetPassword;

public class SetPasswordCommandValidator : AbstractValidator<SetPasswordCommand>
{
    private const int PasswordMinLength = 6;
    public SetPasswordCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Id.Value)
                    .NotEmpty();
            });

        RuleFor(x => x.CurrentPassword)
            .NotNull();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(PasswordMinLength);
    }
}