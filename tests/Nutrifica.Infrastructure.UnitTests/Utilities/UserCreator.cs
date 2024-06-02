using Nutrifica.Domain.ClientAggregate.ValueObjects;
using Nutrifica.Domain.UserAggregate;

namespace Nutrifica.Infrastructure.UnitTests.Utilities;

public class UserCreator
{
    public static User Create()
    {
        return User.Create("username", "firstName", "middleName", 
            "lastname", PhoneNumber.Create("5678"), "email", null);
    }
}