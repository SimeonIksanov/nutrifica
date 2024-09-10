using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Application.Orders.Create;
using Nutrifica.Application.Orders.Get;
using Nutrifica.Application.Orders.GetById;
using Nutrifica.Application.Orders.OrderItems.Add;
using Nutrifica.Application.Orders.OrderItems.Remove;
using Nutrifica.Application.Orders.OrderItems.Update;
using Nutrifica.Application.Orders.Update;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.ProductAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Api.Controllers;

[Route("api/orders")]
public class OrdersController : ApiController
{
    public OrdersController(IMediator mediator) : base(mediator) { }

    #region Manage Order

    /// <summary>
    /// Get order 
    /// </summary>
    /// <param name="ct"></param>
    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetOrder([FromRoute] Guid orderId, CancellationToken ct)
    {
        var query = new GetOrderQuery(OrderId.Create(orderId));
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    /// Get Order list 
    /// </summary>
    /// <param name="ct"></param>
    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] QueryParams queryParams, CancellationToken ct)
    {
        var query = new GetOrdersQuery(queryParams);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    /// Creates Order 
    /// </summary>
    /// <param name="ct"></param>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto request, CancellationToken ct)
    {
        var command = new CreateOrderCommand() { ClientId = ClientId.Create(request.ClientId) };
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetOrder), new { orderId = result.Value }, result.Value)
            : HandleFailure(result);
    }

    /// <summary>
    /// Update order
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="ct"></param>
    [HttpPut("{orderId:guid}")]
    public async Task<IActionResult> UpdateOrder([FromRoute] Guid orderId, [FromBody] OrderUpdateDto request,
        CancellationToken ct)
    {
        if (orderId != request.Id) return BadRequest();

        var command = new UpdateOrderCommand()
        {
            Id = OrderId.Create(orderId),
            Status = request.Status,
            ManagerIds = request.ManagerIds.Select(UserId.Create).ToArray()
        };
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    #endregion

    #region Manage Order Items
    /// <summary>
    /// Create order item 
    /// </summary>
    /// <param name="ct"></param>
    [HttpPost("{orderId:guid}/items")]
    public async Task<IActionResult> AddOrderItem([FromRoute] Guid orderId, [FromBody] OrderItemCreateDto orderItem,
        CancellationToken ct)
    {
        if (orderId != orderItem.OrderId) return BadRequest();
        var command = new CreateOrderItemCommand()
        {
            OrderId = OrderId.Create(orderId),
            ProductId = ProductId.Create(orderItem.ProductId),
            Quantity = orderItem.Quantity
        };
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok() : HandleFailure(result);
    }
    
    /// <summary>
    /// Update order item
    /// </summary>
    /// <param name="ct"></param>
    [HttpPut("{orderId:guid}/items")]
    public async Task<IActionResult> UpdateOrderItem([FromRoute] Guid orderId, [FromBody] OrderItemUpdateDto orderItem,
        CancellationToken ct)
    {
        if (orderId != orderItem.OrderId) return BadRequest();
        var command = new UpdateOrderItemCommand()
        {
            OrderId = OrderId.Create(orderId),
            ProductId = ProductId.Create(orderItem.ProductId),
            Quantity = orderItem.Quantity
        };
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok() : HandleFailure(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct"></param>
    [HttpDelete("{orderId:guid}/items/{productId:int}")]
    public async Task<IActionResult> RemoveOrderItem([FromRoute] Guid orderId, [FromRoute] int productId,
        CancellationToken ct)
    {
        // if (orderId != orderItem.OrderId) return BadRequest();
        var command = new RemoveOrderItemCommand()
        {
            OrderId = OrderId.Create(orderId),
            ProductId = ProductId.Create(productId)
        };
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok() : HandleFailure(result);
    }
    #endregion
}