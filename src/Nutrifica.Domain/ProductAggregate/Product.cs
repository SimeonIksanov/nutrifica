using Nutrifica.Domain.Common.Interfaces;
using Nutrifica.Domain.Common.Models;
using Nutrifica.Domain.ProductAggregate.ValueObjects;

namespace Nutrifica.Domain.ProductAggregate;

public class Product : Entity<ProductId>, IAggregateRoot
{
    public static Product Create(string name, decimal price, string details)
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
    public decimal Price { get; private set; }
    public string Details { get; set; } = string.Empty;
}