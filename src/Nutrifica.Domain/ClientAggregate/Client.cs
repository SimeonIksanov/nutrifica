using Nutrifica.Domain.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Common.Interfaces;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.OrderAggregate.ValueObjects;
using Nutrifica.Domain.UserAggregate.ValueObjects;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Domain.ClientAggregate;

public class Client : Entity<ClientId>, IAggregateRoot
{
    private Client()
    {
    }

    public string FirstName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => string.Join(" ", LastName, FirstName, MiddleName).Trim();

    public Address Address { get; set; } = null!;
    public Comment Comment { get; set; }
    public PhoneNumber PhoneNumber { get; set; } = null!;
    public string Source { get; set; } = string.Empty;
    public HashSet<UserId> Operators { get; private set; } = null!;
    public ICollection<OrderId> OrderIds { get; private set; } = null!;
    public ICollection<PhoneCallId> PhoneCallIds { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }
    public UserId CreatedBy { get; private set; } = null!;
    public Status Status { get; set; }
    public State State { get; set; }

    public static Client Create(string firstName, string middleName, string lastName,
        PhoneNumber phoneNumber, Address address, Comment comment, UserId createdBy, string source)
    {
        ArgumentNullException.ThrowIfNull(phoneNumber);
        ArgumentNullException.ThrowIfNull(createdBy);

        var client = new Client
        {
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Address = address,
            Comment = comment,
            CreatedBy = createdBy,
            Source = source,
            State = State.Active,
            Status = Status.New
        };
        return client;
    }
}