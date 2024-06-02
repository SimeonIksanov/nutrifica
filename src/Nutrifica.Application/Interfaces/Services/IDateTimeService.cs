namespace Nutrifica.Application.Interfaces.Services;

public interface IDateTimeService
{
    DateTimeOffset UtcNow { get; }
}