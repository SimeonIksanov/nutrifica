using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Shared;

namespace Nutrifica.Infrastructure.UnitTests.Utilities;

public class UserCreator
{
    public static User Create()
    {
        return User.Create("username", FirstName.Create("fn"), MiddleName.Create("mn"),
            LastName.Create("lastname"), PhoneNumber.Create("5678"), Email.Create("email"), null);
    }
}
