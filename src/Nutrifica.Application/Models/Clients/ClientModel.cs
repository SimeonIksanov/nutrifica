using Nutrifica.Shared.Enums;

namespace Nutrifica.Application.Models.Clients;

public record ClientModel(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    AddressModel Address,
    string Comment,
    string PhoneNumber,
    string Source,
    State State,
    DateTime CreatedAt);