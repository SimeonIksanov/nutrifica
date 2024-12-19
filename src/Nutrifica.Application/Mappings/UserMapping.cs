using Nutrifica.Api.Contracts.Users;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Aggregates.UserAggregate;

namespace Nutrifica.Application.Mappings;

public static class UserMapping
{
    public static UserModel ToUserModel(this User user) =>
        new UserModel
        {
            Id = user.Id,
            Username = user.Account.Username,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber.Value,
            Role = user.Role,
            Enabled = user.Enabled,
            DisableReason = user.DisableReason,
            SupervisorId = user.SupervisorId,
            CreatedOn = user.CreatedOn,
        };

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
            supervisor?.Id.Value ?? null,
            user.Role,
            user.CreatedOn);
    }

    public static UserDto ToUserDto(this UserModel user)
    {
        return new UserDto(
            user.Id.Value,
            user.Username,
            user.FirstName.Value,
            user.MiddleName.Value,
            user.LastName.Value,
            user.Email.Value,
            user.PhoneNumber,
            user.Enabled,
            user.DisableReason,
            user.SupervisorId?.Value?? null,
            user.Role,
            user.CreatedOn);
    }

    public static UserShortDto ToUserShortDto(this UserShortModel shortModel) =>
        new UserShortDto(shortModel.Id.Value, shortModel.FirstName.Value, shortModel.MiddleName.Value, shortModel.LastName.Value);
}