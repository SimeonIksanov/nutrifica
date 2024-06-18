using Nutrifica.Api.Contracts.Users;
using Nutrifica.Domain.Aggregates.UserAggregate;

namespace Nutrifica.Application.Mappings;

public static class UserMapping
{
    public static UserDto ToUserDto(this User user, User? supervisor)
    {
        return new UserDto(
            user.Id.Value,
            user.Account.Username,
            user.FirstName.Value,
            user.MiddleName.Value,
            user.LastName.Value,
            user.Email.Value,
            user.PhoneNumber.Value,
            user.Enabled,
            user.DisableReason,
            supervisor?.Id.Value ?? Guid.Empty,
            supervisor?.FullName ?? string.Empty,
            user.Role,
            user.CreatedAt);
    }
}