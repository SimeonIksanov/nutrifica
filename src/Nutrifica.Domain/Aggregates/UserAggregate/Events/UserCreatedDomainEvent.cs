using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Interfaces;

namespace Nutrifica.Domain.Aggregates.UserAggregate.Events;

public record UserCreatedDomainEvent(UserId userId) : IDomainEvent;
