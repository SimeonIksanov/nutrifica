using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Services.Products;

public class ProductServiceErrors
{
    public static Error FailedToLoad => new Error("ProductService.FailedToLoad", "Не удалось загрузить список продуктов");
    public static Error FailedToCreate => new Error("ProductService.FailedToCreate", "Не удалось создать продукт");
    public static Error FailedToUpdate => new Error("ProductService.FailedToUpdate", "Не удалось обновить продукт");
}