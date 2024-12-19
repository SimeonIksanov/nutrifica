using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public sealed class Client : Entity<ClientId>, IAggregateRoot, IAuditableEntity
{
    // private HashSet<ClientManager> _clientManager = null!; // Пользователи которые могут манипулировать клиентом
    // private readonly List<Order> _orders = null!;
    // private List<PhoneCall> _phoneCalls = null!;

    // ReSharper disable once UnusedMember.Local
    private Client() { }

    public DateTime CreatedOn { get; set; }
    public UserId CreatedBy { get; set; } = null!;
    public DateTime LastModifiedOn { get; set; }
    public UserId LastModifiedBy { get; set; } = null!;
    public FirstName FirstName { get; set; } = null!;
    public MiddleName MiddleName { get; set; } = null!;
    public LastName LastName { get; set; } = null!;

    public Address Address { get; set; } = null!;
    public Comment Comment { get; set; } = null!;
    public PhoneNumber PhoneNumber { get; set; } = null!;
    public string Source { get; set; } = string.Empty;

    public State State { get; set; }
    // public IReadOnlyCollection<UserId> ManagerIds => _clientManager.Select(cm => cm.UserId).ToList();
    // public IReadOnlyCollection<Order> Orders => _orders.ToList();
    // public IReadOnlyCollection<PhoneCall> PhoneCalls => _phoneCalls.ToList();


    public static Client Create(FirstName firstName, MiddleName middleName, LastName lastName,
        PhoneNumber phoneNumber, Address address, Comment comment, UserId createdBy, string source)
    {
        var client = new Client()
        {
            Id = ClientId.CreateUnique(),
            FirstName = firstName,
            MiddleName = middleName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Address = address,
            Comment = comment,
            Source = source,
            State = State.Active,
            // _clientManager = new(),
            // _phoneCalls = new(),
        };
        // client.SetManagerId(createdBy);
        client.RaiseDomainEvent(new ClientCreatedDomainEvent(client.Id));
        return client;
    }

    // public void AddPhoneCall(PhoneCall phoneCall)
    // {
    //     _phoneCalls.Add(phoneCall);
    // }

    // public void DeletePhoneCall(PhoneCall phoneCall) => _phoneCalls.Remove(phoneCall);

    // public void SetManagerIds(ICollection<UserId> managerIds)
    // {
    //     _clientManager.Clear();
    //     foreach (var managerId in managerIds)
    //         _clientManager.Add(new ClientManager() {ClientId = Id, UserId = managerId});
    // }

    // private void SetManagerId(UserId managerId) => SetManagerIds([managerId]);
}