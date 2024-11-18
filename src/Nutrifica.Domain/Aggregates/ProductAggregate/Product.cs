using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Enums;

namespace Nutrifica.Domain.Aggregates.ProductAggregate;

public sealed class Product : Entity<ProductId>, IAggregateRoot, IAuditableEntity
{
    public static Product Create(string name, Money price, string details)
    {
        return new Product()
        {
            Name = name,
            Price = price,
            Details = details
        };
    }

    private Product() { }

    public string Name { get; set; } = string.Empty;
    public Money Price { get; set; } = null!;
    public string Details { get; set; } = string.Empty;
    public State State { get; set; }
    public DateTime CreatedOn { get; set; }
    public UserId CreatedBy { get; set; } = null!;
    public DateTime LastModifiedOn { get; set; }
    public UserId LastModifiedBy { get; set; } = null!;
}
