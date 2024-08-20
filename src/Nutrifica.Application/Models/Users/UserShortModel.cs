namespace Nutrifica.Application.Models.Users;

public record UserShortModel(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName);