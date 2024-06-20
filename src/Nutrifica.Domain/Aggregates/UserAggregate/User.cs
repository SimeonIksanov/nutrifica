using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.Entities;
using Nutrifica.Domain.Aggregates.UserAggregate.Events;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Domain.Aggregates.UserAggregate;

public sealed class User : Entity<UserId>, IAggregateRoot
{
    public static User Create(
        string username,
        FirstName firstName,
        MiddleName middleName,
        LastName lastname,
        PhoneNumber phoneNumber,
        Email email,
        UserId? supervisorId)
    {
        ArgumentNullException.ThrowIfNull(username);
        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastname);

        var user = new User
        {
            // Username = username,
            Id = UserId.CreateUnique(),
            Account = new UserAccount() { Username = username },
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastname,
            PhoneNumber = phoneNumber,
            Email = email,
            SupervisorId = supervisorId,
            Role = UserRole.Operator,
            CreatedAt = DateTime.UtcNow,
            Enabled = true,
        };

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));
        return user;
    }

    private User()
    {
    }

    public UserAccount Account { get; private set; }

    public bool Enabled { get; private set; }
    public string DisableReason { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public FirstName FirstName { get; set; }
    public MiddleName MiddleName { get; set; }
    public LastName LastName { get; set; }
    public string FullName => string.Join(" ", LastName, FirstName, MiddleName).Trim();

    public Email Email { get; set; }
    public PhoneNumber PhoneNumber { get; set; } = null!;

    public UserId? SupervisorId { get; set; } = null!;
    public UserRole Role { get; set; }

    public void Enable()
    {
        Enabled = true;
        DisableReason = string.Empty;
    }

    public void Disable(string reason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(reason, nameof(reason));
        Enabled = false;
        DisableReason = reason;
    }

    public override string ToString() => FullName;
}