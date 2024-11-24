using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Services.Notifications;

public class NotificationServiceErrors
{
    public static Error FailedToCreate =>
        new Error("NotificationService.FailedToCreate", "Не удалось создать уведомление.");

    public static Error FailedToLoad =>
        new Error("NotificationService.FailedToLoad", "Не удалось загрузить список уведомлений");
}