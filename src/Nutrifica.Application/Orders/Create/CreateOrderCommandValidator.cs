using FluentValidation;

namespace Nutrifica.Application.Orders.Create;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.ClientId)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.ClientId.Value)
                .NotEmpty());
    }
}