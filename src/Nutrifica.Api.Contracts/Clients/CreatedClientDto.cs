namespace Nutrifica.Api.Contracts.Clients;

public sealed record CreatedClientDto(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    AddressDto Address,
    string Comment,
    string PhoneNumber,
    string Source,
    DateTime CreatedAt);