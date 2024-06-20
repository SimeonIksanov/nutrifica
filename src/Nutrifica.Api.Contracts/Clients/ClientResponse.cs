namespace Nutrifica.Api.Contracts.Clients;

public sealed record ClientResponse(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    AddressDto Address,
    string Comment,
    string PhoneNumber,
    string Source,
    DateTime CreatedAt);

public sealed record ClientDetailedResponse(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    AddressDto Address,
    string Comment,
    string PhoneNumber,
    string Source,
    DateTime CreatedAt,
    ICollection<PhoneCallResponse> phoneCalls);

public record PhoneCallResponse(
    int Id,
    DateTime CreatedAt,
    Guid CreatedById,
    string CreatedByName,
    // ICollection<ProductModel> products
    string Comment);