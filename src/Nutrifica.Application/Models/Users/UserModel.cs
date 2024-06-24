using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Models.Users;

public record UserModel(
    Guid Id,
    string Username,
    string FirstName,
    string MiddleName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool Enabled,
    string DisableReason,
    UserFullName? Supervisor,
    UserRole Role,
    DateTime CreatedAt);