using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.Clients.CreatePhoneCall;

public record CreatePhoneCallCommand(
    ClientId clientId,
    string Comment
) : ICommand<PhoneCallDto>;