using FluentValidation;

namespace Nutrifica.Application.Users.Get;

public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(x => x.QueryParams.Page)
            .GreaterThan(1);
        RuleFor(x => x.QueryParams.PageSize)
            .GreaterThan(1);
    }
}