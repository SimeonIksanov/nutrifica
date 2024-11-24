using Nutrifica.Api.Contracts.Notifications;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Services.Notifications;

public interface INotificationService
{
    Task<IResult> CreateAsync(NotificationCreateDto dto, CancellationToken cancellationToken);
    Task<IResult<ICollection<NotificationDto>>> GetAsync(DateTime since, DateTime till, CancellationToken cancellationToken);
}