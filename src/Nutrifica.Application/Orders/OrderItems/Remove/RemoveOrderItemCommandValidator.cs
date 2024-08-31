using FluentValidation;

namespace Nutrifica.Application.Orders.OrderItems.Remove;

public class RemoveOrderItemCommandValidator : AbstractValidator<RemoveOrderItemCommand>
{
    public RemoveOrderItemCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.OrderId.Value).NotEmpty());
        RuleFor(x => x.ProductId)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.ProductId.Value).NotEmpty());
    }
}