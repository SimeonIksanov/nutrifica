using Nutrifica.Api.Contracts.Users;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.Accounts.Create;

public record CreateUserCommand(
    string Username,
    FirstName FirstName,
    MiddleName MiddleName,
    LastName LastName,
    Email Email,
    PhoneNumber PhoneNumber,
    UserId? SupervisorId
) : ICommand<UserDTO>;