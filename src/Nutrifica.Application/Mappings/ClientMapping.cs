using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Domain.Aggregates.ClientAggregate;

namespace Nutrifica.Application.Mappings;

public static class ClientMapping
{
    public static CreatedClientDto ToCreatedClientDto(this Client client)
    {
        return new CreatedClientDto(
            client.Id.Value,
            client.FirstName.Value,
            client.MiddleName.Value,
            client.LastName.Value,
            client.Address.ToAddressDto(),
            client.Comment.Value,
            client.PhoneNumber.Value,
            client.Source,
            client.CreatedAt
        );
    }

    public static UpdatedClientDto ToUpdatedClientDto(this Client client)
    {
        return new UpdatedClientDto(
            client.Id.Value,
            client.FirstName.Value,
            client.MiddleName.Value,
            client.LastName.Value,
            client.Address.ToAddressDto(),
            client.Comment.Value,
            client.PhoneNumber.Value,
            client.Source,
            client.CreatedAt
        );
    }
}