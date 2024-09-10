using FluentValidation;

namespace Nutrifica.Application.Orders.OrderItems.Update;

public class UpdateOrderItemCommandValidator : AbstractValidator<UpdateOrderItemCommand>
{
    public UpdateOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.OrderId.Value).NotEmpty());
        RuleFor(x => x.ProductId)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.ProductId.Value).NotEmpty());
        RuleFor(x => x.Quantity > 0);
        RuleFor(x => x.UnitPrice)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.UnitPrice.Amount).GreaterThan(0));
    }
}
