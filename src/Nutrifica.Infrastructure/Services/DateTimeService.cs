using Nutrifica.Application.Interfaces.Services;

namespace Nutrifica.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}