using Microsoft.Extensions.Logging;

using Nutrifica.Application.Models.Orders;
using Nutrifica.Application.Models.Users;

using Sieve.Services;

namespace Nutrifica.Infrastructure.Services.SortAndFilter;

public class SieveCustomFilterMethods : ISieveCustomFilterMethods
{
    private readonly ILogger<SieveCustomFilterMethods> _logger;

    public SieveCustomFilterMethods(ILogger<SieveCustomFilterMethods> logger)
    {
        _logger = logger;
    }

    public IQueryable<UserModel> Email(IQueryable<UserModel> source, string op, string[] values)
    {
        if (values is null || values.Length == 0 || values.Length > 1) return source;
        var result = source.Where(p => p.Email == Domain.Shared.Email.Create(values[0]));
        return result;
    }

    public IQueryable<OrderModel> ClientId(IQueryable<OrderModel> source, string op, string[] values)
    {
        if (values is null || values.Length == 0 || values.Length > 1) return source;
        if (Guid.TryParse(values[0], out Guid clientId))
            return source.Where(p =>
                p.Client.Id == Domain.Aggregates.ClientAggregate.ValueObjects.ClientId.Create(clientId));
        return source;
    }
}