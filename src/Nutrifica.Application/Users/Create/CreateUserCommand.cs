using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.Users.Create;

public record CreateUserCommand(
    string Username,
    FirstName FirstName,
    MiddleName MiddleName,
    LastName LastName,
    Email Email,
    PhoneNumber PhoneNumber,
    UserId? SupervisorId
) : ICommand<UserResponse>;