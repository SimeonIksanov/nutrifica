using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Services.Users;

static class UserServiceErrors
{
    public static Error FailedToCreate => new Error("UserService.FailedToCreate", "Не удалось создать пользователя");
    public static Error FailedToLoad => new Error("UserService.FailedToLoad", "Не удалось загрузить список пользователей");
    public static Error FailedToLoadSingle => new Error("UserService.FailedToLoadSingle", "Не удалось загрузить данные пользователя");
}