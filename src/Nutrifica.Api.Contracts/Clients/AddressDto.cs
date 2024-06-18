namespace Nutrifica.Api.Contracts.Clients;

public sealed record AddressDto(
    string ZipCode,
    string Country,
    string Region,
    string City,
    string Street,
    string Comment
);