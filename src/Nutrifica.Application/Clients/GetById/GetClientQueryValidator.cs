using FluentValidation;

namespace Nutrifica.Application.Clients.GetById;

public class GetClientQueryValidator : AbstractValidator<GetClientQuery>
{
    public GetClientQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.Id.Value).NotEmpty());
    }
}