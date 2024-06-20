using Nutrifica.Shared.Enums;

namespace Nutrifica.Api.Contracts.Users.Requests;

public record UserUpdateRequest(
    Guid Id,
    string Username,
    string FirstName,
    string MiddleName,
    string LastName,
    string Email,
    string PhoneNumber,
    UserRole Role,
    bool Enabled,
    string DisableReason,
    Guid? SupervisorId);