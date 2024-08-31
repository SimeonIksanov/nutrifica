using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Orders.Get;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, PagedList<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetOrdersQueryHandler(IOrderRepository orderRepository, ICurrentUserService currentUserService)
    {
        _orderRepository = orderRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<PagedList<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orderPagedList = await _orderRepository
            .GetByFilterAsync(request.QueryParams, cancellationToken);
        var responseList = PagedList<OrderDto>.Create(
            orderPagedList.Items.Select(x => x.ToOrderDto()).ToList(),
            orderPagedList.Page,
            orderPagedList.PageSize,
            orderPagedList.TotalCount);
        return Result.Success(responseList);
    }
}