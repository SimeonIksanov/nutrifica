using FluentValidation;

namespace Nutrifica.Application.Orders.GetById;

public class GetOrderQueryValidator : AbstractValidator<GetOrderQuery>
{
    public GetOrderQueryValidator()
    {
        RuleFor(x => x.OrderId)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.OrderId.Value)
                    .NotEmpty();
            });
    }
}