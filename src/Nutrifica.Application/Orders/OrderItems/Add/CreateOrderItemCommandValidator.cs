using FluentValidation;

namespace Nutrifica.Application.Orders.OrderItems.Add;

public class CreateOrderItemCommandValidator : AbstractValidator<CreateOrderItemCommand>
{
    public CreateOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.OrderId.Value).NotEmpty());
        RuleFor(x => x.ProductId)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.ProductId.Value).NotEmpty());
        RuleFor(x => x.Quantity > 0);
    }
}