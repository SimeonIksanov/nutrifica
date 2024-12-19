using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Orders.Update;

public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Result<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, _currentUserService.UserId, cancellationToken);
        if (order is null)
            return Result.Failure<OrderDto>(OrderError.OrderNotFound);

        order.Status = request.Status;
        // order.SetManagerIds(request.ManagerIds);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var orderModel = await _orderRepository.GetOrderModelByIdAsync(order.Id, _currentUserService.UserId, cancellationToken);
        return Result.Success(orderModel!.ToOrderDto());
    }
}