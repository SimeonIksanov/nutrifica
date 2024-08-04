using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Clients.Update;

public record UpdateClientCommand(
    ClientId Id,
    FirstName FirstName,
    MiddleName MiddleName,
    LastName LastName,
    Address Address,
    Comment Comment,
    PhoneNumber PhoneNumber,
    string Source,
    State State) : ICommand<ClientResponse>;