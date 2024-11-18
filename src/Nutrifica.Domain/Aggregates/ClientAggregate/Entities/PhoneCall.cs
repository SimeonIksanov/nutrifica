using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.ClientAggregate.Entities;

public class PhoneCall : Entity<int>, IAuditableEntity
{
    public static PhoneCall Create(string comment)
    {
        var call = new PhoneCall
        {
            Comment = comment
        };
        return call;
    }

    private PhoneCall() { }

    public string Comment { get; set; } = string.Empty;

    // public ICollection<ProductId> ProductIds { get; set; } = null!;
    public UserId CreatedBy { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public UserId LastModifiedBy { get; set; } = null!;
    public DateTime LastModifiedOn { get; set; }
}