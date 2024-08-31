using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.OrderAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Orders.Create;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientRepository _clientRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork,
        IClientRepository clientRepository,
        IOrderRepository orderRepository,
        ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
        _orderRepository = orderRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<Guid>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var hasClient = await _clientRepository.HasClientWithIdAsync(request.ClientId, cancellationToken);
        if (!hasClient) return Result.Failure<Guid>(ClientErrors.ClientNotFound);

        var order = Order.Create(request.ClientId, _currentUserService.UserId);
        _orderRepository.Add(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order.Id.Value);
    }
}