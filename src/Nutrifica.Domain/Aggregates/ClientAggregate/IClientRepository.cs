using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Domain.Aggregates.ClientAggregate;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(ClientId clientId, CancellationToken ct = default);
    void Add(Client client);
}
