// ReSharper disable NotAccessedPositionalProperty.Global

using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Users.Responses;

public record UserResponse(
    Guid Id,
    string Username,
    string FirstName,
    string MiddleName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool Enabled,
    string DisableReason,
    Guid? SupervisorId,
    string SupervisorName,
    UserRole Role,
    DateTime CreatedAt
);