using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Domain;

public record class ClientCreatedDomainEvent(ClientId ClientId) : IDomainEvent;
