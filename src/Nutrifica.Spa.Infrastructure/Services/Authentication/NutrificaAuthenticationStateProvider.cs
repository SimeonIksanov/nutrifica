using System.Security.Claims;

using Microsoft.AspNetCore.Components.Authorization;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.Authentication;

public class NutrificaAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly IAuthenticationService _authService;

    public NutrificaAuthenticationStateProvider(IAuthenticationService authService)
    {
        _authService = authService;
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
    }

    public User CurrentUser { get; private set; } = new();

    public void Dispose() => AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = new ClaimsPrincipal();
        User? user = await _authService.FetchUserFromBrowser();
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

        IResult<User> result = await _authService.SendAuthenticateRequestAsync(request, ct);
        if (result.IsSuccess)
        {
            principal = new ClaimsPrincipal(result.Value.ToClaimsPrincipal());
            CurrentUser = result.Value;
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        return result;
    }

    public async Task LogoutAsync(CancellationToken cancellationToken)
    {
        await _authService.SendLogoutRequest(cancellationToken);
        await _authService.ClearBrowserUserData();
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