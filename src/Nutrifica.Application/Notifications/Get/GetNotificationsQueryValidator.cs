using FluentValidation;

namespace Nutrifica.Application.Notifications.Get;

public class GetNotificationsQueryValidator : AbstractValidator<GetNotificationsQuery>
{
    public GetNotificationsQueryValidator()
    {
        RuleFor(x => x.Since).NotEmpty();
        RuleFor(x => x.Till).NotEmpty();
    }
}