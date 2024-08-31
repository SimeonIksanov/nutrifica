using FluentValidation;

namespace Nutrifica.Application.Products.Update;

public class ProductUpdateCommandValidator : AbstractValidator<ProductUpdateCommand>
{
    public ProductUpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Id.Value).NotEmpty();
            });
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Details).NotNull();
        RuleFor(x => x.Price)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Price.Amount).GreaterThan(0);
            });
    }
}