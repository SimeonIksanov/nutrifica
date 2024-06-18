using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;

namespace Nutrifica.Application.Mappings;

public static class AddressMapping
{
    public static AddressDto ToAddressDto(this Address address)
    {
        return new AddressDto(
            address.ZipCode,
            address.Country,
            address.Region,
            address.City,
            address.Street,
            address.Comment);
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