// ReSharper disable NotAccessedPositionalProperty.Global

namespace Nutrifica.Api.Contracts.Users.Requests;

public record UserCreateRequest(
    string Username,
    string FirstName,
    string MiddleName,
    string LastName,
    string Email,
    string PhoneNumber,
    Guid? SupervisorId);