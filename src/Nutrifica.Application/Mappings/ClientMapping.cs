using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Models.Clients;
using Nutrifica.Domain.Aggregates.ClientAggregate;

namespace Nutrifica.Application.Mappings;

public static class ClientMapping
{
    public static ClientDto ToClientDto(this Client client)
    {
        return new ClientDto
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
            CreatedAt = client.CreatedOn
        };
    }

    public static ClientDto ToClientDto(this ClientModel client)
    {
        return new ClientDto
        {
            Id = client.Id,
            FirstName = client.FirstName,
            MiddleName = client.MiddleName,
            LastName = client.LastName,
            Address = client.Address.ToAddressDto(),
            PhoneNumber = client.PhoneNumber,
            Comment = client.Comment,
            CreatedAt = client.CreatedOn,
            Source = client.Source,
            State = client.State,
            Managers = client.Managers.Select(x=>x.ToUserShortDto()).ToArray()
        };
    }

    public static PhoneCallDto ToPhoneCallDto(this PhoneCallModel phoneCall)
    {
        return new PhoneCallDto
        {
            Comment = phoneCall.Comment,
            CreatedOn = phoneCall.CreatedOn,
            CreatedBy = phoneCall.CreatedBy.ToUserShortDto(),
            Id = phoneCall.Id,
        };
    }
}