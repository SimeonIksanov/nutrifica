using Nutrifica.Application.Abstractions.Messaging;

namespace Nutrifica.Application;

public record LogoutCommand(string Jwt, string RefreshToken, string IpAddress) : ICommand;
