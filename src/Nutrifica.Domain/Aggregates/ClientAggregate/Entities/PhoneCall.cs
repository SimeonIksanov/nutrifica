using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Domain.Aggregates.ClientAggregate.Entities;

public class PhoneCall : Entity<int>
{
    public static PhoneCall Create(
        //ICollection<ProductId> productIds,
        string comment,
        UserId createdBy)
    {
        var call = new PhoneCall
        {
            CreatedBy = createdBy,
            // ProductIds = productIds, 
            Comment = comment
        };
        return call;
    }

    private PhoneCall() { }

    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public UserId CreatedBy { get; init; } = null!;
    // public ICollection<ProductId> ProductIds { get; set; } = null!;
}