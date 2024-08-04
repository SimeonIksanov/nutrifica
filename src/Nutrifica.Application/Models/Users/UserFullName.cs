namespace Nutrifica.Application.Models.Users;

public record UserFullName(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName);