using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.OrderAggregate.Entities;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Orders.OrderItems.Update;

public class UpdateOrderItemCommandHandler : ICommandHandler<UpdateOrderItemCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateOrderItemCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(UpdateOrderItemCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, _currentUserService.UserId, cancellationToken);
        if (order is null)
            return Result.Failure(OrderError.OrderNotFound);

        var newOrderItem = CreateOrderItem(command);
        order.UpdateItem(newOrderItem);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    private OrderItem CreateOrderItem(UpdateOrderItemCommand command)
    {
        var orderItem = OrderItem.Create(command.ProductId, command.Quantity, command.UnitPrice);
        return orderItem;
    }
}