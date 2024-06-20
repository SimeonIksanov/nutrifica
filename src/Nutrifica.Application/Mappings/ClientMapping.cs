using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Models.Clients;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.Entities;

namespace Nutrifica.Application.Mappings;

public static class ClientMapping
{
    public static ClientResponse ToClientResponse(this Client client)
    {
        return new ClientResponse(
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

    public static ClientResponse ToClientResponse(this ClientModel client)
    {
        return new ClientResponse(
            client.Id,
            client.FirstName,
            client.MiddleName,
            client.LastName,
            client.Address.ToAddressDto(),
            client.Comment,
            client.PhoneNumber,
            client.Source,
            client.CreatedAt
        );
    }

    public static ClientDetailedResponse ToClientResponse(this ClientDetailedModel client)
    {
        return new ClientDetailedResponse(
            Id: client.Id,
            FirstName: client.FirstName,
            MiddleName: client.MiddleName,
            LastName: client.LastName,
            Address: client.Address.ToAddressDto(),
            Comment: client.Comment,
            PhoneNumber: client.PhoneNumber,
            Source: client.Source,
            CreatedAt: client.CreatedAt,
            phoneCalls: client.phoneCalls.Select(x=>x.ToPhoneCallResponse()).ToArray());
    }

    public static PhoneCallResponse ToPhoneCallResponse(this PhoneCallModel phoneCall)
    {
        return new PhoneCallResponse(
            phoneCall.Id,
            phoneCall.CreatedAt,
            phoneCall.CreatedById,
            phoneCall.CreatedByName,
            phoneCall.Comment);
    }
}