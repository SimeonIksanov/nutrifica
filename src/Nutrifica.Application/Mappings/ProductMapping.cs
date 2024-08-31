using Nutrifica.Api.Contracts.Products;
using Nutrifica.Application.Models.Products;

namespace Nutrifica.Application.Mappings;

public static class ProductMapping
{
    public static ProductDto ToProductDto(this ProductModel model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        Details = model.Details,
        Price = model.Price.ToMoneyDto(),
        State = model.State
    };
}