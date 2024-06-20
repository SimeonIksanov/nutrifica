using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.Entities;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public sealed class Client : Entity<ClientId>, IAggregateRoot
{
    private readonly HashSet<UserId> _managerIds = null!; // Пользователи которые могут манипулировать клиентом
    private readonly List<OrderId> _orderIds = null!;
    private readonly List<PhoneCall> _phoneCalls = null!;
    private Client() { }

    private Client(UserId createdBy)
    {
        _managerIds = new HashSet<UserId> { createdBy };
        State = State.Active;
        Status = Status.New;
        CreatedAt = DateTime.UtcNow;
    }

    public FirstName FirstName { get; set; }
    public MiddleName MiddleName { get; set; }
    public LastName LastName { get; set; }
    public string FullName => string.Join(" ", LastName, FirstName, MiddleName).Trim();

    public Address Address { get; set; } = null!;
    public Comment Comment { get; set; }
    public PhoneNumber PhoneNumber { get; set; } = null!;
    public string Source { get; set; } = string.Empty;
    public IReadOnlyCollection<UserId> ManagerIds => _managerIds.ToList();
    public IReadOnlyCollection<OrderId> OrderIds => _orderIds.ToList();
    public IReadOnlyCollection<PhoneCall> PhoneCalls => _phoneCalls.ToList();

    public DateTime CreatedAt { get; private set; }
    public UserId CreatedBy { get; private set; } = null!;
    public Status Status { get; set; }
    public State State { get; set; }

    public static Client Create(FirstName firstName, MiddleName middleName, LastName lastName,
        PhoneNumber phoneNumber, Address address, Comment comment, UserId createdBy, string source)
    {
        var client = new Client(createdBy)
        {
            Id = ClientId.CreateUnique(),
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Address = address,
            Comment = comment,
            CreatedBy = createdBy,
            Source = source
        };
        client.RaiseDomainEvent(new ClientCreatedDomainEvent(client.Id));
        return client;
    }
}