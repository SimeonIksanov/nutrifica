using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.OrderAggregate.Entities;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Orders.OrderItems.Update;

public class UpdateOrderItemCommandHandler : ICommandHandler<UpdateOrderItemCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderItemCommandHandler(IOrderRepository orderRepository, IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateOrderItemCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
        if (order is null)
            return Result.Failure(OrderError.OrderNotFound);

        var newOrderItem = await CreateOrderItem(command, cancellationToken);
        order.UpdateItem(newOrderItem);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private async Task<OrderItem> CreateOrderItem(UpdateOrderItemCommand command, CancellationToken cancellationToken)
    {
        var unitPrice = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        var orderItem = OrderItem.Create(command.ProductId, command.Quantity, unitPrice.Price);
        return orderItem;
    }
}