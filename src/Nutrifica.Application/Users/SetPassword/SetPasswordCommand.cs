using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Application.Users.SetPassword;

public record SetPasswordCommand(UserId Id, string CurrentPassword, string NewPassword) : ICommand;