using MediatR;

using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;

namespace Nutrifica.Application.Orders.GetById;

public record GetOrderQuery(OrderId OrderId) : IQuery<OrderDto>;