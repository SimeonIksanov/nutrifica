using Nutrifica.Api.Contracts.Users;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Accounts.Update;

public record UpdateUserCommand(
    UserId Id,
    string Username,
    FirstName FirstName,
    MiddleName MiddleName,
    LastName LastName,
    Email Email,
    PhoneNumber PhoneNumber,
    UserRole Role,
    bool Enabled,
    string DisableReason,
    UserId? SupervisorId) : ICommand<UserDto>;