using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Services.Storage;

namespace Nutrifica.Spa.Infrastructure.Services.Authentication;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly IDataStorage _storage;

    public UserService(HttpClient httpClient, IDataStorage storage)
    {
        _httpClient = httpClient;
        _storage = storage;
    }

    public async Task<IResult<User>> SendAuthenticateRequestAsync(TokenRequest request, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync(
            requestUri: Routes.AuthenticationEndpoints.Login,
            value: request,
            options: JsonSerializerOptions.Default,
            cancellationToken: ct);

        if (!response.IsSuccessStatusCode)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: ct);
            return Result.Failure<User>(new Error(response?.ReasonPhrase?? "Unknown error", problemDetails?.Detail??"unknown error"));
        }

        var tokenResponse = await ParseResponse(response, ct);
        if (tokenResponse is null)
        {
            return Result.Failure<User>(new Error("Unknown error", "unknown error"));;
        }

        var claimPrincipal = CreateClaimPrincipalFromToken(tokenResponse.Jwt);
        var user = User.FromClaimsPrincipal(claimPrincipal);
        PersistUserToBrowser(tokenResponse);
        return Result.Success(user);
    }

    public async Task<User?> SendRefreshTokensRequestAsync(CancellationToken ct = default)
    {
        if (_storage.TryGetValue(nameof(RefreshTokenRequest.Jwt), out string? jwt)
            && _storage.TryGetValue(nameof(RefreshTokenRequest.RefreshToken), out string? refreshToken))
        {
            var response = await _httpClient.PostAsJsonAsync(
                requestUri: Routes.AuthenticationEndpoints.RefreshToken,
                value: new RefreshTokenRequest(jwt!, refreshToken!),
                JsonSerializerOptions.Default,
                ct);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            TokenResponse? tokenResponse = await ParseResponse(response, ct);
            if (tokenResponse is null)
            {
                return null;
            }

            var claimPrincipal = CreateClaimPrincipalFromToken(tokenResponse.Jwt);
            var user = User.FromClaimsPrincipal(claimPrincipal);
            PersistUserToBrowser(tokenResponse);
            return user;
        }

        return null;
    }


    public User? FetchUserFromBrowser()
    {
        if (_storage.TryGetValue(nameof(TokenResponse.Jwt), out string? jwt))
        {
            var claimsPrincipal = CreateClaimPrincipalFromToken(jwt!);
            return User.FromClaimsPrincipal(claimsPrincipal);
        }

        // return User.FromClaimsPrincipal(new ClaimsPrincipal(new ClaimsIdentity()));
        return null;
    }

    public void ClearBrowserUserData()
    {
        _storage.Clear();
    }

    private void PersistUserToBrowser(TokenResponse tokenResponse)
    {
        _storage.SetValue(nameof(TokenResponse.Jwt), tokenResponse.Jwt);
        _storage.SetValue(nameof(TokenResponse.RefreshToken), tokenResponse.RefreshToken);
    }

    private ClaimsPrincipal CreateClaimPrincipalFromToken(string jwt)
    {
        var identity = new ClaimsIdentity();
        var tokenHandler = new JwtSecurityTokenHandler();
        if (tokenHandler.CanReadToken(jwt))
        {
            var jwtSecutityToken = tokenHandler.ReadJwtToken(jwt);
            identity = new ClaimsIdentity(jwtSecutityToken.Claims, "Nutrifica");
        }

        return new ClaimsPrincipal(identity);
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