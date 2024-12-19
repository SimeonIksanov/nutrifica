using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.PhoneCallAggregate;

public class PhoneCall : Entity<PhoneCallId>, IAggregateRoot, IAuditableEntity
{
    private PhoneCall() { }

    public static PhoneCall Create(ClientId clientId, string comment)
    {
        var call = new PhoneCall { Id = PhoneCallId.CreateUnique(), ClientId = clientId, Comment = comment };
        return call;
    }


    public ClientId ClientId { get; private set; } = null!;
    public string Comment { get; set; } = string.Empty;
    // public PhoneCallType Type { get; private set; }

    // public ICollection<ProductId> ProductIds { get; set; } = null!;
    public UserId CreatedBy { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public UserId LastModifiedBy { get; set; } = null!;
    public DateTime LastModifiedOn { get; set; }
}

// public enum PhoneCallType
// {
//     Outgoing,
//     Incoming,
// }