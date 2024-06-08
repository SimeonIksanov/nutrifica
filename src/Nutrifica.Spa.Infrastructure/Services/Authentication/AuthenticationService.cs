using System.Net.Http.Json;
using System.Text.Json;
using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<IResult> LoginAsync(TokenRequest request, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync(
            requestUri: Routes.AuthenticationEndpoints.Login,
            value: request,
            options: JsonSerializerOptions.Default, 
            cancellationToken: ct);
        var resultAsJson = await response.Content.ReadAsStringAsync(ct);
        var result = JsonSerializer.Deserialize<IResult<TokenResponse>>(resultAsJson, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        if (result is null || result.Failure)
            return Result.Fail(result is null ? "Не удалось войти" : result.Message);
        return Result.Ok();
    }

    public Task<IResult> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public Task<IResult> LogOutAsync(LogoutRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}