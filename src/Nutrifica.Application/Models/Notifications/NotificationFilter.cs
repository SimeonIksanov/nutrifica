using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Application.Models.Notifications;

public record NotificationFilter(DateTime Since, DateTime Till, UserId Requester);