using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Shared;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Orders.Get;

public record GetOrdersQuery(QueryParams QueryParams) : IQuery<PagedList<OrderDto>>;