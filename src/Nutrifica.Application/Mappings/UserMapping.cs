using Nutrifica.Api.Contracts.Users;
using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Aggregates.UserAggregate;

namespace Nutrifica.Application.Mappings;

public static class UserMapping
{
    public static UserResponse ToUserResponse(this User user, User? supervisor)
    {
        return new UserResponse(
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

    public static UserResponse ToUserResponse(this UserModel user)
    {
        return new UserResponse(
            user.Id,
            user.Username,
            user.FirstName,
            user.MiddleName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Enabled,
            user.DisableReason,
            user.SupervisorId,
            user.SupervisorName,
            user.Role,
            user.CreatedAt);
    }
}