using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.Mappings;

public static class AddressMapping
{
    public static AddressDto ToAddressDto(this Address address)
    {
        return new AddressDto
        {
            City = address.City,
            Comment = address.Comment,
            Country = address.Country,
            Region = address.Region,
            Street = address.Street,
            ZipCode = address.ZipCode,
        };
    }

    public static Address ToAddress(this AddressDto dto)
    {
        return new Address
        {
            ZipCode = dto.ZipCode,
            Country = dto.Country,
            Region = dto.Region,
            City = dto.City,
            Street = dto.Street,
            Comment = dto.Comment
        };
    }
}