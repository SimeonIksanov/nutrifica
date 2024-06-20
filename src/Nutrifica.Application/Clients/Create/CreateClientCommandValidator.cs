using FluentValidation;

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
                .MaximumLength(20));

        RuleFor(x => x.MiddleName)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.MiddleName.Value)
                .NotNull()
                .MaximumLength(20));

        RuleFor(x => x.LastName)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.LastName.Value)
                .NotNull()
                .MaximumLength(20));

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
                .MaximumLength(20));

        RuleFor(x => x.Source).NotEmpty().MaximumLength(20);

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
        RuleFor(x => x.City).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Country).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Comment).NotNull();
        RuleFor(x => x.Region).NotNull().MaximumLength(50);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ZipCode).NotEmpty().MaximumLength(6);
    }
}

