using FluentValidation;

using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.Clients.Create;

public class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.City)
            .NotNull()
            .MaximumLength(ClientAddressConstants.CityMaxLength);
        RuleFor(x => x.Country)
            .NotNull()
            .MaximumLength(ClientAddressConstants.CountryMaxLength);
        RuleFor(x => x.Comment)
            .NotNull();
        RuleFor(x => x.Region)
            .NotNull()
            .MaximumLength(ClientAddressConstants.RegionMaxLength);
        RuleFor(x => x.Street)
            .NotNull()
            .MaximumLength(ClientAddressConstants.StreetMaxLength);
        RuleFor(x => x.ZipCode)
            .NotNull()
            .MaximumLength(ClientAddressConstants.ZipCodeMaxLength);
    }
}