using FluentValidation;

using Nutrifica.Application.Clients.Create;
using Nutrifica.Application.Users;

namespace Nutrifica.Application.Clients.Update;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.Id.Value)
                .NotEmpty());

        RuleFor(x => x.FirstName)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.FirstName.Value)
                .NotNull()
                .MaximumLength(UserConstants.FirstNameMaxLength));

        RuleFor(x => x.MiddleName)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.MiddleName.Value)
                .NotNull()
                .MaximumLength(UserConstants.MiddleNameMaxLength));

        RuleFor(x => x.LastName)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.LastName.Value)
                .NotNull()
                .MaximumLength(UserConstants.LastNameMaxLength));

        RuleFor(x => x.Address)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.Address)
                .SetValidator(new AddressValidator()));

        RuleFor(x => x.Comment)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.Comment.Value)
                .NotNull());

        RuleFor(x => x.PhoneNumber)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.PhoneNumber.Value)
                .NotEmpty()
                .MaximumLength(UserConstants.PhoneNumberMaxLength));

        RuleFor(x => x.Source)
            .NotEmpty()
            .MaximumLength(ClientConstants.SourceMaxLength);
    }
}