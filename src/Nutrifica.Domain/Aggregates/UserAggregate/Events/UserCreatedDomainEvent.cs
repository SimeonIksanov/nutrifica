using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Domain.Aggregates.UserAggregate.Events;

public record UserCreatedDomainEvent(UserId userId) : IDomainEvent;
