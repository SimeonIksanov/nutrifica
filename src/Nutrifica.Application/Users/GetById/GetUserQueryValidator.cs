using FluentValidation;

namespace Nutrifica.Application.Users.GetById;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .DependentRules(() => RuleFor(x => x.Id.Value).NotEmpty());
    }
}