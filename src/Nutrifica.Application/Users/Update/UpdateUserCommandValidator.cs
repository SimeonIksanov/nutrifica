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
            .DependentRules(() =>
            {
                RuleFor(x => x.Id.Value).NotEmpty();
            });

        RuleFor(x => x.Username)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.FirstName)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.FirstName.Value)
                    .NotEmpty()
                    .MaximumLength(50);
            });

        RuleFor(x => x.MiddleName)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.MiddleName.Value)
                    .NotEmpty()
                    .MaximumLength(50);
            });

        RuleFor(x => x.LastName)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.LastName.Value)
                    .NotEmpty()
                    .MaximumLength(50);
            });

        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.PhoneNumber.Value)
                    .MaximumLength(15);
            });

        RuleFor(x => x.Email)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Email.Value)
                    .MaximumLength(50);
            });

        RuleFor(x => x.DisableReason)
            .NotEmpty().When(x => x.Enabled is false)
            .WithMessage("No disable reason specified");

        RuleFor(x => x.DisableReason)
            .Empty().When(x => x.Enabled)
            .WithMessage("No disable reason should be specified");
    }
}