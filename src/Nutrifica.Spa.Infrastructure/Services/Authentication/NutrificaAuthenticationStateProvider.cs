using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.Authentication;

public class NutrificaAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly IUserService _userService;

    public NutrificaAuthenticationStateProvider(IUserService userService)
    {
        _userService = userService;
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
    }

    public User CurrentUser { get; private set; } = new();

    public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = new ClaimsPrincipal();
        User? user = await _userService.FetchUserFromBrowser();
        if (user is null)
        {
            return new AuthenticationState(principal);
        }
        principal = user.ToClaimsPrincipal();
        CurrentUser = user;

        return new AuthenticationState(principal);
    }

    public async Task<IResult<User>> LoginAsync(TokenRequest request, CancellationToken ct)
    {
        var principal = new ClaimsPrincipal();

        var result = await _userService.SendAuthenticateRequestAsync(request, ct);
        if (result.IsSuccess)
        {
            principal = new ClaimsPrincipal(result.Value.ToClaimsPrincipal());
            CurrentUser = result.Value;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        return result;
    }

    public async Task Logout(CancellationToken cancellationToken)
    {
        await _userService.SendLogoutRequest(cancellationToken);
        await _userService.ClearBrowserUserData();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
    }

    private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
    {
        AuthenticationState? authenticationState = await task;
        if (authenticationState is not null)
        {
            CurrentUser = User.FromClaimsPrincipal(authenticationState.User);
        }
    }
}