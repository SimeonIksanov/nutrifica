using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Services.Orders;

public static class OrderServiceErrors
{
    public static Error FailedToLoad => new Error("OrderService.FailedToLoad", "Не удалось загрузить список заказов");
    public static Error FailedToCreate => new Error("OrderService.FailedToCreate", "Не удалось создать заказ");
    public static Error FailedToUpdate => new Error("OrderService.FailedToUpdate", "Не удалось обновить заказ");
    public static Error FailedToAddItem => new Error("OrderService.FailedToAddItem", "Не удалось добавить в заказ");
    public static Error FailedToUpdateItem => new Error("OrderService.FailedToAddItem", "Не удалось обновить позицию в заказе");
}