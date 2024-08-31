using FluentValidation;

namespace Nutrifica.Application.Products.GetById;

public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.ProductId.Value).NotEmpty());
    }
}