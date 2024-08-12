using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Services.Clients;

static class ClientServiceErrors
{
    public static Error FailedToCreate => new Error("ClientService.FailedToCreate", "Не удалось создать клиента.");
    public static Error FailedToCreatePhoneCall => new Error("ClientService.FailedToCreatePhoneCall", "Не удалось создать звонок.");
    public static Error FailedToUpdate => new Error("ClientService.FailedToUpdate", "Не удалось обновить данные клиента.");
    public static Error FailedToLoad => new Error("ClientService.FailedToLoad", "Не удалось загрузить список клиентов.");
    public static Error FailedToLoadPhoneCalls => new Error("ClientService.FailedToLoadPhoneCalls", "Не удалось загрузить список звонков.");
    public static Error FailedToUpdatePhoneCalls => new Error("ClientService.FailedToUpdatePhoneCalls", "Не удалось обновить звонок.");
    public static Error FailedToDeletePhoneCalls => new Error("ClientService.FailedToDeletePhoneCalls", "Не удалось удалить звонок.");
}