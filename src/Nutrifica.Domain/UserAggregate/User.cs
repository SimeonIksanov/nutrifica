using Nutrifica.Domain.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Common.Interfaces;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.UserAggregate.Enums;
using Nutrifica.Domain.UserAggregate.ValueObjects;

namespace Nutrifica.Domain.UserAggregate;

public class User : Entity<UserId>, IAggregateRoot
{
    public static User Create(string username, string firstName, string middleName, string lastname,
        PhoneNumber phoneNumber, string email, UserId? supervisorId)
    {
        ArgumentException.ThrowIfNullOrEmpty(username);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastname);

        var newUser = new User
        {
            Username = username,
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastname,
            PhoneNumber = phoneNumber,
            Email = email,
            Enabled = true,
            Role = Role.Operator,
            SupervisorId = supervisorId,
        };
        return newUser;
    }

    private User()
    {
    }

    public string Username { get; private set; } = string.Empty;

    public bool Enabled { get; set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime? FiredAt { get; private set; }

    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => string.Join(" ", LastName, FirstName, MiddleName).Trim();

    public string Email { get; set; } = string.Empty;
    public PhoneNumber PhoneNumber { get; set; } = null!;

    public UserId? SupervisorId { get; set; } = null!;
    public ICollection<ClientId> ClientIds { get; private set; } = null!;
    public Role Role { get; set; }

    public override string ToString()
    {
        return FullName;
    }
}