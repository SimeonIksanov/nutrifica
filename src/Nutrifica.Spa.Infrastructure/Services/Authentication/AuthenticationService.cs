using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

using Blazored.LocalStorage;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Routes;

namespace Nutrifica.Spa.Infrastructure.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _storage;
    private HttpClient? _httpClient;

    public AuthenticationService(IHttpClientFactory httpClientFactory, ILocalStorageService storage)
    {
        _httpClientFactory = httpClientFactory;
        _storage = storage;
    }

    public async Task<IResult<User>> SendAuthenticateRequestAsync(TokenRequest request, CancellationToken ct)
    {
        HttpResponseMessage response;
        try
        {
            response = await CreateHttpClient().PostAsJsonAsync(
                AuthenticationEndpoints.Login,
                request,
                ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return Result.Failure<User>(new Error("HttpRequestFailure", ex.Message));
        }

        if (!response.IsSuccessStatusCode)
        {
            var problemDetails = await ParseResponse<ProblemDetails>(response, ct);
            return Result.Failure<User>(new Error(response.ReasonPhrase ?? "HttpRequestUnsuccessful",
                problemDetails?.Detail ?? "unknown error"));
        }

        var tokenResponse = await ParseResponse<TokenResponse>(response, ct);
        if (tokenResponse is null)
        {
            return Result.Failure<User>(new Error("Jwt.ParseError", "unknown error"));
        }

        await PersistUserToBrowser(tokenResponse, ct);
        return Result.Success(ConvertToUser(tokenResponse.Jwt));
    }

    public async Task<bool> IsJwtValidAsync(CancellationToken ct)
    {
        string? jwt = await GetJwtFromStorage(ct);
        return IsValidToken(jwt);
    }

    public async Task<IResult> SendRefreshTokensRequestAsync(CancellationToken ct)
    {
        string? jwt = await GetJwtFromStorage(ct);
        string? refreshToken = await GetRefreshTokenFromStorage(ct);
        return await SendRefreshTokensRequestAsync(jwt, refreshToken, ct);
    }

    public async Task SendLogoutRequest(CancellationToken ct)
    {
        string? jwt = await GetJwtFromStorage(ct);
        string? refreshToken = await GetRefreshTokenFromStorage(ct);

        if (string.IsNullOrWhiteSpace(jwt) || string.IsNullOrWhiteSpace(refreshToken))
        {
            await ClearBrowserUserData();
            return;
        }

        var requestBody = new LogoutRequest(jwt, refreshToken);
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, AuthenticationEndpoints.LogOut);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            request.Content = JsonContent.Create(requestBody);
            var response = await CreateHttpClient()
                .SendAsync(request, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<User?> FetchUserFromBrowser()
    {
        string? jwt = await GetJwtFromStorage();

        if (string.IsNullOrWhiteSpace(jwt)) return null;

        ClaimsPrincipal claimsPrincipal = CreateClaimPrincipalFromToken(jwt);
        if (IsValidToken(jwt))
        {
            return User.FromClaimsPrincipal(claimsPrincipal);
        }

        string? refreshToken = await GetRefreshTokenFromStorage();
        if (!string.IsNullOrWhiteSpace(refreshToken) &&
            (await SendRefreshTokensRequestAsync(jwt, refreshToken)).IsSuccess)
        {
            return ConvertToUser(await GetJwtFromStorage());
        }

        await _storage.ClearAsync();
        return null;
    }

    public async Task ClearBrowserUserData() => await _storage.ClearAsync();

    public async Task<string?> GetJwtFromStorage(CancellationToken ct = default) =>
        await _storage.GetItemAsStringAsync(nameof(TokenResponse.Jwt), ct);

    private HttpClient CreateHttpClient() => _httpClient ??= _httpClientFactory.CreateClient("apiBackendWoHandlers");

    private async Task<IResult> SendRefreshTokensRequestAsync(string jwt, string refreshToken,
        CancellationToken ct = default)
    {
        HttpResponseMessage response;
        try
        {
            response = await CreateHttpClient().PostAsJsonAsync(
                AuthenticationEndpoints.RefreshToken,
                new RefreshTokenRequest(jwt, refreshToken),
                ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<User>(new Error("HttpRequestFailure", ex.Message));
        }

        if (!response.IsSuccessStatusCode)
        {
            var problemDetails = await ParseResponse<ProblemDetails>(response, ct);
            return Result.Failure(new Error(response.ReasonPhrase ?? "HttpRequestUnsuccessful",
                problemDetails?.Detail ?? "unknown error"));
        }

        var tokenResponse = await ParseResponse<TokenResponse>(response, ct);
        if (tokenResponse is null)
        {
            return Result.Failure(new Error("Jwt.ParseError", "unknown error"));
        }

        return Result.Success();
    }

    private async Task PersistUserToBrowser(TokenResponse tokenResponse, CancellationToken ct = default)
    {
        await _storage.SetItemAsStringAsync(nameof(TokenResponse.Jwt), tokenResponse.Jwt, ct);
        await _storage.SetItemAsStringAsync(nameof(TokenResponse.RefreshToken), tokenResponse.RefreshToken, ct);
    }

    private ClaimsPrincipal CreateClaimPrincipalFromToken(string? jwt)
    {
        var identity = new ClaimsIdentity();
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.CanReadToken(jwt))
        {
            JwtSecurityToken? jwtSecurityToken = tokenHandler.ReadJwtToken(jwt);
            identity = new ClaimsIdentity(jwtSecurityToken.Claims, "Nutrifica");
        }

        return new ClaimsPrincipal(identity);
    }

    private bool IsValidToken(string? jwt)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.CanReadToken(jwt))
        {
            JwtSecurityToken? jwtSecurityToken = tokenHandler.ReadJwtToken(jwt);
            return jwtSecurityToken.ValidFrom < DateTime.UtcNow && DateTime.UtcNow < jwtSecurityToken.ValidTo;
        }

        return false;
    }

    private static async Task<T?> ParseResponse<T>(HttpResponseMessage response, CancellationToken ct)
    {
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var tokenResponse = await response.Content.ReadFromJsonAsync<T>(opts, ct);
        return tokenResponse;
    }

    private async Task<string?> GetRefreshTokenFromStorage(CancellationToken ct = default) =>
        await _storage.GetItemAsStringAsync(nameof(TokenResponse.RefreshToken), ct);

    private User ConvertToUser(string? jwt)
    {
        ClaimsPrincipal claimPrincipal = CreateClaimPrincipalFromToken(jwt);
        return User.FromClaimsPrincipal(claimPrincipal);
    }
}