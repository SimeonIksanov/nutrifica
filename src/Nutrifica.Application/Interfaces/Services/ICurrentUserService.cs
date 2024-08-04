using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Application.Interfaces.Services;

public interface ICurrentUserService
{
    UserId UserId { get; }
}