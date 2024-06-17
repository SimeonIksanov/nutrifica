namespace Nutrifica.Application.Abstractions.Clock;

public interface IDateTimeProvider
{
    // DateTimeOffset UtcNow { get; }
    DateTime UtcNow { get; }
}
