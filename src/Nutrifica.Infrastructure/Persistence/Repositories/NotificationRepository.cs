using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Notifications;
using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Aggregates.NotificationAggregate;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(Notification notification)
    {
        _context.Set<Notification>().Add(notification);
    }

    public async Task<ICollection<NotificationModel>> GetAsync(NotificationFilter filter,
        CancellationToken cancellationToken)
    {
        ICollection<NotificationModel> models = await _context.Set<Notification>()
            .Where(x =>
                filter.Since <= x.DateTime
                && x.DateTime <= filter.Till
                && (x.CreatedBy == filter.Requester
                    || x.RecipientId == filter.Requester))
            .Select(x => new NotificationModel
            {
                Id = x.Id.Value,
                DateTime = x.DateTime,
                Message = x.Message,
                CreatedOn = x.CreatedOn,
                Recipient = (from user in _context.Users
                    where user.Id == x.RecipientId
                    select new UserShortModel(
                        user.Id.Value,
                        user.FirstName.Value,
                        user.MiddleName.Value,
                        user.LastName.Value)).FirstOrDefault(),
                CreatedBy = (from user in _context.Users
                    where user.Id == x.CreatedBy
                    select new UserShortModel(
                        user.Id.Value,
                        user.FirstName.Value,
                        user.MiddleName.Value,
                        user.LastName.Value)).FirstOrDefault(),
            })
            .ToListAsync(cancellationToken);
        return models;
    }
}