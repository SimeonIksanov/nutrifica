using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;

namespace Nutrifica.Application.Users.ResetPassword;

public record ResetPasswordCommand(UserId Id, string NewPassword) : ICommand;