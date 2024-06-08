using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Common.Interfaces;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Domain.Aggregates.ProductAggregate;

public sealed class Product : Entity<ProductId>, IAggregateRoot
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

    private Product()
    {

    }

    public string Name { get; set; } = string.Empty;
    public Money Price { get; private set; }
    public string Details { get; set; } = string.Empty;
}
