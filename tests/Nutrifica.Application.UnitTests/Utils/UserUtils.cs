using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Application.UnitTests.Utils;

public class UserUtils
{
    public static User CreateUser()
    {
        var user = User.Create(
            "admin",
            FirstName.Create("fn"),
            MiddleName.Create("mn"),
            LastName.Create("ln"),
            null!, null!, null);
        user.Account.Salt = "value";
        user.Account.PasswordHash = "value";
        return user;
    }
}