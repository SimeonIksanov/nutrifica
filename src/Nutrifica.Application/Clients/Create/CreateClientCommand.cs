using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.Clients.Create;

public record CreateClientCommand(
    FirstName FirstName,
    MiddleName MiddleName,
    LastName LastName,
    Address Address,
    Comment Comment,
    PhoneNumber PhoneNumber,
    string Source,
    UserId CreatedBy) : ICommand<CreatedClientDto>;