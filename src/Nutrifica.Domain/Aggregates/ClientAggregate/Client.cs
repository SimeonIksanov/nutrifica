using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.Entities;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public sealed class Client : Entity<ClientId>, IAggregateRoot, IAuditableEntity
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
    }

    public DateTime CreatedOn { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTime LastModifiedOn { get; set; }
    public UserId LastModifiedBy { get; set; }
    public FirstName FirstName { get; set; }
    public MiddleName MiddleName { get; set; }
    public LastName LastName { get; set; }

    public Address Address { get; set; } = null!;
    public Comment Comment { get; set; }
    public PhoneNumber PhoneNumber { get; set; } = null!;
    public string Source { get; set; } = string.Empty;
    public Status Status { get; set; }
    public State State { get; set; }
    public IReadOnlyCollection<UserId> ManagerIds => _managerIds.ToList();
    public IReadOnlyCollection<OrderId> OrderIds => _orderIds.ToList();
    public IReadOnlyCollection<PhoneCall> PhoneCalls => _phoneCalls.ToList();


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
            Source = source
        };
        client.RaiseDomainEvent(new ClientCreatedDomainEvent(client.Id));
        return client;
    }

    public void AddPhoneCall(PhoneCall phoneCall)
    {
        _phoneCalls.Add(phoneCall);
    }

    public void DeletePhoneCall(PhoneCall phoneCall) => _phoneCalls.Remove(phoneCall);

    public class Builder
    {
        private Client _client;

        public Builder(UserId createdBy)
        {
            _client = new Client(createdBy);
        }

        public Builder WithFirstName(FirstName value)
        {
            _client.FirstName = value;
            return this;
        }

        public Builder WithLastName(LastName value)
        {
            _client.LastName = value;
            return this;
        }

        public Builder WithMiddleName(MiddleName value)
        {
            _client.MiddleName = value;
            return this;
        }

        public Builder WithPhoneNumber(PhoneNumber value)
        {
            _client.PhoneNumber = value;
            return this;
        }

        public Builder WithAddress(Address value)
        {
            _client.Address = value;
            return this;
        }

        public Builder WithComment(Comment value)
        {
            _client.Comment = value;
            return this;
        }

        public Builder WithSource(string value)
        {
            _client.Source = value;
            return this;
        }
    }
}