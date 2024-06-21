using FluentValidation;

using Nutrifica.Application.Users;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.Clients.Create;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
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

        RuleFor(x => x.CreatedBy)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.CreatedBy.Value)
                .NotEmpty());
    }
}

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.City)
            .NotEmpty()
            .MaximumLength(ClientAddressConstants.CityMaxLength);
        RuleFor(x => x.Country)
            .NotEmpty()
            .MaximumLength(ClientAddressConstants.CountryMaxLength);
        RuleFor(x => x.Comment)
            .NotNull();
        RuleFor(x => x.Region)
            .NotNull()
            .MaximumLength(ClientAddressConstants.RegionMaxLength);
        RuleFor(x => x.Street)
            .NotEmpty()
            .MaximumLength(ClientAddressConstants.StreetMaxLength);
        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .MaximumLength(ClientAddressConstants.ZipCodeMaxLength);
    }
}

