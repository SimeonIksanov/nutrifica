using Nutrifica.Api.Contracts.Users;
using Nutrifica.Api.Contracts.Users.Responses;
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
            supervisor is null ? null : new UserFullNameResponse(supervisor.Id.Value,supervisor.FirstName.Value,supervisor.MiddleName.Value,supervisor.LastName.Value),
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
            user.Supervisor?.ToUserFullNameResponse(),
            user.Role,
            user.CreatedAt);
    }

    public static UserFullNameResponse ToUserFullNameResponse(this UserFullName fullName) =>
        new UserFullNameResponse(fullName.Id, fullName.FirstName, fullName.MiddleName, fullName.LastName);
}