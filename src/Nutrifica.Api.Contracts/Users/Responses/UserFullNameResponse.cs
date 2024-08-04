namespace Nutrifica.Api.Contracts.Users.Responses;

public record UserFullNameResponse(Guid Id, string FirstName, string MiddleName, string LastName);