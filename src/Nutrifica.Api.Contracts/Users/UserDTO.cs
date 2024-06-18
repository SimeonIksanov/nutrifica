using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Users;

public record UserDTO(
    Guid Id,
    string Username,
    string FirstName,
    string MiddleName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool Enabled,
    string DisableReason,
    Guid SupervisorId,
    string Supervisor,
    UserRole Role,
    DateTime CreatedAt
);