using FluentValidation;

namespace Nutrifica.Application.Clients.Get;

public class GetClientsQueryValidator : AbstractValidator<GetClientsQuery>
{
    public GetClientsQueryValidator()
    {
        RuleFor(x => x.QueryParams.Page)
            .GreaterThan(1);
        RuleFor(x => x.QueryParams.PageSize)
            .GreaterThan(1);
    }
}