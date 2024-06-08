using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Common.Interfaces;

namespace Nutrifica.Domain;

public record class ClientCreatedDomainEvent(ClientId ClientId) : IDomainEvent;
