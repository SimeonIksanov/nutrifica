using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Domain.Aggregates.ClientAggregate.Events;

public record class ClientCreatedDomainEvent(ClientId ClientId, UserId UserId) : IDomainEvent;
