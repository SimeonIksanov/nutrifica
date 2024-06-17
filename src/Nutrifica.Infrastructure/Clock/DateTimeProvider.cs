using Nutrifica.Application.Abstractions.Clock;

namespace Nutrifica.Infrastructure.Clock;

public class DateTimeProvider : IDateTimeProvider
{
    // public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public DateTime UtcNow => DateTime.UtcNow;
}
