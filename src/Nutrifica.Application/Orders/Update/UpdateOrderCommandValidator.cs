using FluentValidation;

namespace Nutrifica.Application.Orders.Update;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .DependentRules(() => { RuleFor(x => x.Id.Value).NotEmpty(); });
        RuleFor(x => x.ManagerIds)
            .ForEach(x => x.NotEmpty());
    }
}