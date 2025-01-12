using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.Events;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public sealed class Client : Entity<ClientId>, IAggregateRoot, IAuditableEntity
{
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
        };
        client.RaiseDomainEvent(new ClientCreatedDomainEvent(client.Id, createdBy));
        return client;
    }
}