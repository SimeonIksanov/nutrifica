using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Clients;
using Nutrifica.Application.Models.Orders;
using Nutrifica.Application.Models.Users;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.OrderAggregate;
using Nutrifica.Domain.Aggregates.OrderAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Infrastructure.Services.SortAndFilter;
using Nutrifica.Shared.Wrappers;

using Sieve.Services;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly ISieveProcessor _sieveProcessor;

    public OrderRepository(AppDbContext context, ISieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<Order?> GetByIdAsync(OrderId orderId, UserId managerId, CancellationToken ct = default)
    {
        return await (
            from order in _context.Orders
            where order.Id.Equals(orderId)
            select order
        ).FirstOrDefaultAsync(ct);
    }

    public async Task<OrderModel?> GetOrderModelByIdAsync(OrderId orderId, UserId managerId,
        CancellationToken ct = default)
    {
        var query = (from order in _context.Orders.AsNoTracking()
            join creator in _context.Users on order.CreatedBy equals creator.Id
            join client in _context.Clients on order.ClientId equals client.Id
            where order.Id == orderId
            select new OrderModel
            {
                Id = order.Id,
                CreatedOn = order.CreatedOn,
                TotalSum = order.TotalSum,
                Status = order.Status,
                CreatedBy = new UserShortModel
                {
                    Id = creator.Id,
                    FirstName = creator.FirstName,
                    MiddleName = creator.MiddleName,
                    LastName = creator.LastName
                },
                Client = new ClientShortModel
                {
                    Id = client.Id,
                    FirstName = client.FirstName,
                    MiddleName = client.MiddleName,
                    LastName = client.LastName
                },
                OrderItems = (
                    from orderItem in order.OrderItems
                    join product in _context.Products on orderItem.ProductId equals product.Id
                    select new OrderItemModel
                    {
                        Id = orderItem.Id,
                        Quantity = orderItem.Quantity,
                        ProductId = orderItem.ProductId,
                        UnitPrice = orderItem.UnitPrice,
                        ProductName = product.Name,
                        ProductDetails = product.Details
                    }).ToList()
            });
        var orderModel = await query.FirstOrDefaultAsync(ct);
        return orderModel;
    }

    public async Task<PagedList<OrderModel>> GetByFilterAsync(QueryParams queryParams, UserId managerId,
        CancellationToken cancellationToken)
    {
        var query = (from order in _context.Orders.AsNoTracking()
            join creator in _context.Users on order.CreatedBy equals creator.Id
            join client in _context.Clients on order.ClientId equals client.Id
            select new OrderModel
            {
                Id = order.Id,
                CreatedOn = order.CreatedOn,
                TotalSum = order.TotalSum,
                Status = order.Status,
                CreatedBy =
                    new UserShortModel
                    {
                        Id = creator.Id,
                        FirstName = creator.FirstName,
                        MiddleName = creator.MiddleName,
                        LastName = creator.LastName
                    },
                Client = new ClientShortModel
                {
                    Id = client.Id,
                    FirstName = client.FirstName,
                    MiddleName = client.MiddleName,
                    LastName = client.LastName
                },
                OrderItems = (
                    from orderItem in order.OrderItems
                    join product in _context.Products on orderItem.ProductId equals product.Id
                    select new OrderItemModel
                    {
                        Id = orderItem.Id,
                        Quantity = orderItem.Quantity,
                        ProductId = orderItem.ProductId,
                        UnitPrice = orderItem.UnitPrice,
                        ProductName = product.Name,
                        ProductDetails = product.Details
                    }
                ).ToList()
            });
        var pagedList =
            await query.SieveToPagedListAsync(_sieveProcessor, queryParams.ToSieveModel(), cancellationToken);
        return pagedList;
    }

    public void Add(Order order)
    {
        _context.Set<Order>().Add(order);
    }
}