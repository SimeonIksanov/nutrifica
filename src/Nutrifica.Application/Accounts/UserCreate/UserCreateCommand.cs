using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.UserAggregate;

namespace Nutrifica.Application.Accounts.UserCreate;

public class UserCreateCommand : ICommand<User>
{
    public string Username { get; set; }
    public string FirstName { get; set; }
}
