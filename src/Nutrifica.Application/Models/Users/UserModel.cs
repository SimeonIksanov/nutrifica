using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Models.Users;

public record UserModel
{
    public UserId Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public FirstName FirstName { get; set; } = null!;
    public MiddleName MiddleName { get; set; } = null!;
    public LastName LastName { get; set; } = null!;
    public Email Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public bool Enabled { get; set; }
    public string DisableReason { get; set; } = null!;
    public UserId? SupervisorId { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedOn { get; set; }
}