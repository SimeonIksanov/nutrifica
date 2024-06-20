using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Application.Users.ChangePassword;

public record ChangePasswordCommand(UserId Id, string CurrentPassword, string NewPassword) : ICommand;