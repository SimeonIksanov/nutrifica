using FluentValidation;

namespace Nutrifica.Application.Accounts.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
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
    }
}