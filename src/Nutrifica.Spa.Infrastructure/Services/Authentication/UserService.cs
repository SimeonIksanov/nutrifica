using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

using Blazored.LocalStorage;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services.Authentication;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService  _storage;

    public UserService(HttpClient httpClient, ILocalStorageService  storage)
    {
        _httpClient = httpClient;
        _storage = storage;
    }

    public async Task<IResult<User>> SendAuthenticateRequestAsync(TokenRequest request, CancellationToken ct)
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(
                requestUri: Routes.AuthenticationEndpoints.Login,
                value: request,
                cancellationToken: ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result.Failure<User>(new Error("HttpRequestFailure", ex.Message));
        }

        if (!response.IsSuccessStatusCode)
        {
            var problemDetails = await response
                .Content
                .ReadFromJsonAsync<ProblemDetails>(cancellationToken: ct);
            return Result.Failure<User>(new Error(response.ReasonPhrase ?? "HttpRequestUnsuccessful",
                problemDetails?.Detail ?? "unknown error"));
        }

        var tokenResponse = await ParseResponse(response, ct);
        if (tokenResponse is null)
        {
            return Result.Failure<User>(new Error("Jwt.ParseError", "unknown error"));
        }

        var claimPrincipal = CreateClaimPrincipalFromToken(tokenResponse.Jwt);
        var user = User.FromClaimsPrincipal(claimPrincipal);
        await PersistUserToBrowser(tokenResponse, ct);
        return Result.Success(user);
    }

    public async Task<IResult<User>> TryRefreshTokensRequestAsync(CancellationToken ct = default)
    {
        string? jwt = await _storage.GetItemAsStringAsync(nameof(RefreshTokenRequest.Jwt), ct);
        string? refreshToken = await _storage.GetItemAsStringAsync(nameof(RefreshTokenRequest.RefreshToken), ct);

        if (string.IsNullOrWhiteSpace(jwt) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return Result.Failure<User>(
                new Error("Unauthenticated", "Unauthenticated or authentication timeout expired."));
        }

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(
                requestUri: Routes.AuthenticationEndpoints.RefreshToken,
                value: new RefreshTokenRequest(jwt, refreshToken),
                ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<User>(new Error("HttpRequestFailure", ex.Message));
        }
        if (!response.IsSuccessStatusCode)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: ct);
            return Result.Failure<User>(new Error(response.ReasonPhrase ?? "HttpRequestUnsuccessful",
                problemDetails?.Detail ?? "unknown error"));
        }

        TokenResponse? tokenResponse = await ParseResponse(response, ct);
        if (tokenResponse is null)
        {
            return Result.Failure<User>(new Error("Jwt.ParseError", "unknown error"));
        }

        var claimPrincipal = CreateClaimPrincipalFromToken(tokenResponse.Jwt);
        var user = User.FromClaimsPrincipal(claimPrincipal);
        await PersistUserToBrowser(tokenResponse, ct);
        return Result.Success(user);
    }

    public async Task SendLogoutRequest(CancellationToken ct)
    {
        string? jwt = await _storage.GetItemAsStringAsync(nameof(RefreshTokenRequest.Jwt), ct);
        string? refreshToken = await _storage.GetItemAsStringAsync(nameof(RefreshTokenRequest.RefreshToken), ct);

        if (string.IsNullOrWhiteSpace(jwt) || string.IsNullOrWhiteSpace(refreshToken))
        {
            await ClearBrowserUserData();
        }

        LogoutRequest request = new LogoutRequest(jwt!, refreshToken!);
        try
        {
            var _ = await _httpClient.PostAsJsonAsync(Routes.AuthenticationEndpoints.LogOut, request, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<User?> FetchUserFromBrowser()
    {
        string? jwt = await _storage.GetItemAsStringAsync(nameof(TokenResponse.Jwt));
        if (string.IsNullOrWhiteSpace(jwt))
            return null;

        var claimsPrincipal = CreateClaimPrincipalFromToken(jwt);
        if (IsValidToken(jwt))
        {
            return User.FromClaimsPrincipal(claimsPrincipal);
        }

        var result = await TryRefreshTokensRequestAsync();
        if (result.IsSuccess)
        {
            return result.Value;
        }

        await _storage.ClearAsync();
        return null;
    }

    public async Task ClearBrowserUserData()
    {
        await _storage.ClearAsync();
    }

    private async Task PersistUserToBrowser(TokenResponse tokenResponse, CancellationToken ct = default)
    {
        await _storage.SetItemAsStringAsync(nameof(TokenResponse.Jwt), tokenResponse.Jwt, ct);
        await _storage.SetItemAsStringAsync(nameof(TokenResponse.RefreshToken), tokenResponse.RefreshToken, ct);
    }

    private ClaimsPrincipal CreateClaimPrincipalFromToken(string jwt)
    {
        var identity = new ClaimsIdentity();
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.CanReadToken(jwt))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwt);
            identity = new ClaimsIdentity(jwtSecurityToken.Claims, "Nutrifica");
        }

        return new ClaimsPrincipal(identity);
    }

    private bool IsValidToken(string jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.CanReadToken(jwt))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwt);
            return jwtSecurityToken.ValidFrom < DateTime.UtcNow && DateTime.UtcNow < jwtSecurityToken.ValidTo;
        }

        return false;
    }

    private static async Task<TokenResponse?> ParseResponse(HttpResponseMessage response, CancellationToken ct)
    {
        var tokenResponse = await response
            .Content
            .ReadFromJsonAsync<TokenResponse>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
                ct);
        return tokenResponse;
    }
}