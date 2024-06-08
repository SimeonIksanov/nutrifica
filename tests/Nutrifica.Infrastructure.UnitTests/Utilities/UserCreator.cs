using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;

namespace Nutrifica.Infrastructure.UnitTests.Utilities;

public class UserCreator
{
    public static User Create()
    {
        return User.Create("username", "firstName", "middleName", 
            "lastname", PhoneNumber.Create("5678"), "email", null);
    }
}
