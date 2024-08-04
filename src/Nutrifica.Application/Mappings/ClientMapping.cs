using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Models.Clients;
using Nutrifica.Domain.Aggregates.ClientAggregate;

namespace Nutrifica.Application.Mappings;

public static class ClientMapping
{
    public static ClientResponse ToClientResponse(this Client client)
    {
        return new ClientResponse
        {
            Id = client.Id.Value,
            FirstName = client.FirstName.Value,
            MiddleName = client.MiddleName.Value,
            LastName = client.LastName.Value,
            Address = client.Address.ToAddressDto(),
            Comment = client.Comment.Value,
            PhoneNumber = client.PhoneNumber.Value,
            State = client.State,
            Source = client.Source,
            CreatedAt = client.CreatedOn,
        };
    }

    public static ClientResponse ToClientResponse(this ClientModel client)
    {
        return new ClientResponse
        {
            Id = client.Id,
            FirstName = client.FirstName,
            MiddleName = client.MiddleName,
            LastName = client.LastName,
            Address = client.Address.ToAddressDto(),
            PhoneNumber = client.PhoneNumber,
            Comment = client.Comment,
            CreatedAt = client.CreatedAt,
            Source = client.Source,
            State = client.State,
        };
    }

    public static PhoneCallResponse ToPhoneCallResponse(this PhoneCallModel phoneCall)
    {
        return new PhoneCallResponse
        {
            Comment = phoneCall.Comment,
            CreatedOn = phoneCall.CreatedOn,
            CreatedBy = phoneCall.CreatedBy.ToUserFullNameResponse(),
            Id = phoneCall.Id,
        };
    }
}