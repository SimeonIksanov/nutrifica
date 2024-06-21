using FluentValidation;

namespace Nutrifica.Application.Users.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        // RuleLevelCascadeMode = CascadeMode.Stop;
        // ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("ru");
        RuleFor(x => x.Id)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.Id.Value).NotEmpty());

        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(UserConstants.UsernameMaxLength);

        RuleFor(x => x.FirstName)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.FirstName.Value)
                .NotEmpty()
                .MaximumLength(UserConstants.FirstNameMaxLength));

        RuleFor(x => x.MiddleName)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.MiddleName.Value)
                .MaximumLength(UserConstants.MiddleNameMaxLength));

        RuleFor(x => x.LastName)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.LastName.Value)
                .NotEmpty()
                .MaximumLength(UserConstants.LastNameMaxLength));

        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.PhoneNumber.Value)
                .MaximumLength(UserConstants.PhoneNumberMaxLength));

        RuleFor(x => x.Email)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.Email.Value)
                .MaximumLength(UserConstants.EmailMaxLength));

        RuleFor(x => x.DisableReason)
            .NotEmpty().When(x => x.Enabled is false)
            .WithMessage("No disable reason specified");

        RuleFor(x => x.DisableReason)
            .Empty().When(x => x.Enabled)
            .WithMessage("No disable reason should be specified");
    }
}