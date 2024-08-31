using FluentValidation;

namespace Nutrifica.Application.Products.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
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