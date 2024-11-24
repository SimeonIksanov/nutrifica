using Nutrifica.Application.Models.Notifications;
using Nutrifica.Domain.Aggregates.NotificationAggregate;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface INotificationRepository
{
    void Add(Notification notification);
    Task<ICollection<NotificationModel>> GetAsync(NotificationFilter filter, CancellationToken cancellationToken);
}