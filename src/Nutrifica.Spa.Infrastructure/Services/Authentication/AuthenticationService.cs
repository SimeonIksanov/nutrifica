using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Routes;

namespace Nutrifica.Spa.Infrastructure.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ITokenService _tokenService;
    private HttpClient? _httpClient;

    public AuthenticationService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
    {
        _httpClientFactory = httpClientFactory;
        _tokenService = tokenService;
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
            Console.WriteLine(ex);
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

    public async Task<IResult> SendRefreshTokensRequestAsync(CancellationToken ct)
    {
        string jwt = await _tokenService.GetAccessTokenAsync(ct);
        string refreshToken = await _tokenService.GetRefreshTokenAsync(ct);
        return await SendRefreshTokensRequestAsync(jwt, refreshToken, ct);
    }

    public async Task SendLogoutRequest(CancellationToken ct)
    {
        string jwt = await _tokenService.GetAccessTokenAsync(ct);
        string refreshToken = await _tokenService.GetRefreshTokenAsync(ct);

        if (string.IsNullOrWhiteSpace(jwt) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return;
        }

        var requestBody = new LogoutRequest(jwt, refreshToken);
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, AuthenticationEndpoints.LogOut);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            request.Content = JsonContent.Create(requestBody);
            _ = await CreateHttpClient()
                .SendAsync(request, ct);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public async Task<User?> FetchUserFromBrowser()
    {
        string jwt = await _tokenService.GetAccessTokenAsync(default);
        if (string.IsNullOrWhiteSpace(jwt))
        {
            return null;
        }

        ClaimsPrincipal claimsPrincipal = CreateClaimPrincipalFromToken(jwt);
        if (!await _tokenService.IsAccessTokenExpiredAsync())
        {
            return User.FromClaimsPrincipal(claimsPrincipal);
        }

        string refreshToken = await _tokenService.GetRefreshTokenAsync(default);
        if (!string.IsNullOrWhiteSpace(refreshToken) &&
            (await SendRefreshTokensRequestAsync(jwt, refreshToken)).IsSuccess)
        {
            return ConvertToUser(await _tokenService.GetAccessTokenAsync(default));
        }

        await _tokenService.ClearAsync(default);
        return null;
    }

    public async Task ClearBrowserUserData() => await _tokenService.ClearAsync(default);

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

        await PersistUserToBrowser(tokenResponse, ct);

        return Result.Success();
    }

    private async Task PersistUserToBrowser(TokenResponse tokenResponse, CancellationToken ct = default)
    {
        await _tokenService.SaveAccessTokenAsync(tokenResponse.Jwt, ct);
        await _tokenService.SaveRefreshTokenAsync(tokenResponse.RefreshToken, ct);
    }

    private ClaimsPrincipal CreateClaimPrincipalFromToken(string jwt)
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

    private static async Task<T?> ParseResponse<T>(HttpResponseMessage response, CancellationToken ct)
    {
        var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var tokenResponse = await response.Content.ReadFromJsonAsync<T>(opts, ct);
        return tokenResponse;
    }

    private User ConvertToUser(string jwt)
    {
        ClaimsPrincipal claimPrincipal = CreateClaimPrincipalFromToken(jwt);
        return User.FromClaimsPrincipal(claimPrincipal);
    }
}