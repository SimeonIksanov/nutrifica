using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Abstractions.Messaging;

namespace Nutrifica.Application.Authentication.Login;

public class LoginCommand : ICommand<TokenResponse>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
}
