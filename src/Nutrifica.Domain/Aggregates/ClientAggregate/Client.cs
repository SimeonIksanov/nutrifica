using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Interfaces;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public sealed class Client : Entity<ClientId>, IAggregateRoot
{
    private Client() { }

    public FirstName FirstName { get; set; }
    public MiddleName MiddleName { get; set; }
    public LastName LastName { get; set; }
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

    public static Client Create(FirstName firstName, MiddleName middleName, LastName lastName,
        PhoneNumber phoneNumber, Address address, Comment comment, UserId createdBy, string source)
    {
        ArgumentNullException.ThrowIfNull(phoneNumber);
        ArgumentNullException.ThrowIfNull(createdBy);

        var client = new Client
        {
            Id = ClientId.CreateUnique(),
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
        client.RaiseDomainEvent(new ClientCreatedDomainEvent(client.Id));
        return client;
    }
}
