using FluentValidation;

namespace Nutrifica.Application.Users.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
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
                // .NotEmpty()
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
    }
}