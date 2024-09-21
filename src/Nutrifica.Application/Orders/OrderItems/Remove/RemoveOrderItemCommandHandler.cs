using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Orders.OrderItems.Remove;

public class RemoveOrderItemCommandHandler : ICommandHandler<RemoveOrderItemCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public RemoveOrderItemCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result> Handle(RemoveOrderItemCommand command, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, _currentUserService.UserId, cancellationToken);
        if (order is null) return Result.Failure(OrderError.OrderNotFound);

        var orderItem = order.OrderItems.FirstOrDefault(x => x.ProductId == command.ProductId);
        if (orderItem is null)
            return Result.Success();
        order.RemoveItem(orderItem);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}