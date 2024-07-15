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
    UserRole Role,
    DateTime CreatedAt
)
{
    public Guid Id { get; set; } = Id;
    public string Username { get; set; } = Username;
    public string FirstName { get; set; } = FirstName;
    public string MiddleName { get; set; } = MiddleName;
    public string LastName { get; set; } = LastName;
    public string Email { get; set; } = Email;
    public string PhoneNumber { get; set; } = PhoneNumber;
    public bool Enabled { get; set; } = Enabled;
    public string DisableReason { get; set; } = DisableReason;
    public Guid? SupervisorId { get; set; } = SupervisorId;
    public UserRole Role { get; set; } = Role;
    public DateTime CreatedAt { get; set; } = CreatedAt;

    public string FullName => string.Join(" ", LastName, FirstName, MiddleName);
}