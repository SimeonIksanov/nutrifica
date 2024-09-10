using Nutrifica.Api.Contracts.Orders;
using Nutrifica.Application.Models.Orders;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.Mappings;

public static class OrderMapping
{
    public static OrderDto ToOrderDto(this OrderModel model) =>
        new()
        {
            Id = model.Id,
            CreatedBy = model.CreatedBy.ToUserShortDto(),
            CreatedOn = model.CreatedOn,
            Status = model.Status,
            TotalSum = model.TotalSum.ToMoneyDto(),
            Client = model.Client.ToUserShortDto(),
            Managers = model.Managers.Select(x => x.ToUserShortDto()).ToArray(),
            OrderItems = model.OrderItems.Select(x => x.ToOrderItemDto()).ToArray()
        };

    public static OrderItemDto ToOrderItemDto(this OrderItemModel model) =>
        new()
        {
            Id = model.Id,
            Quantity = model.Quantity,
            UnitPrice = model.UnitPrice.ToMoneyDto(),
            ProductId = model.ProductId,
            ProductName = model.ProductName,
            ProductDetails = model.ProductDetails
        };

    public static MoneyDto ToMoneyDto(this Money model) => new()
    {
        Amount = model.Amount, Currency = new CurrencyDto { Code = model.Currency.Code }
    };

    public static Money ToMoney(this MoneyDto dto) => new(dto.Amount, Currency.FromCode(dto.Currency.Code));
}