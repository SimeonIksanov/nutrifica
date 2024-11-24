using FluentValidation;

namespace Nutrifica.Application.Notifications.Create;

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(x => x.Message).NotEmpty();
        RuleFor(x=>x.DateTime).NotEmpty();
    }
}