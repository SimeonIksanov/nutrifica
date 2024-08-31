using Nutrifica.Api.Contracts.Products;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;

namespace Nutrifica.Application.Products.GetById;

public record GetProductQuery(ProductId ProductId) : IQuery<ProductDto>;