using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Interfaces;

namespace Nutrifica.Application.CommandAndQueries.Authentication.Login;

public class LoginCommand : ICommand<TokenResponse>
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}