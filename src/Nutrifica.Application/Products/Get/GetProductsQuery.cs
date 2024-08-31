using Nutrifica.Api.Contracts.Products;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Shared;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Products.Get;

public record GetProductsQuery(QueryParams QueryParams) : IQuery<PagedList<ProductDto>>;