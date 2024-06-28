using System.Net.Http.Json;

using Microsoft.AspNetCore.Components.Authorization;

using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Routes;
using Nutrifica.Spa.Infrastructure.Services.Authentication;

namespace Nutrifica.Spa.Infrastructure.Services.Users;

public class UserService : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly NutrificaAuthenticationStateProvider _stateProvider;
    private HttpClient? _httpClient;

    public UserService(IHttpClientFactory httpClientFactory, NutrificaAuthenticationStateProvider stateProvider)
    {
        _httpClientFactory = httpClientFactory;
        _stateProvider = stateProvider;
    }

    public async Task<IResult<PagedList<UserResponse>>> Get(CancellationToken cancellationToken)
    {
        var requestUri = UsersEndpoints.Get;
        try
        {
            var pagedList = await GetHttpClient()
                .GetFromJsonAsync<PagedList<UserResponse>>(requestUri, cancellationToken);
            return Result.Success(pagedList!);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список пользователей: {ex.Message}");
            return Result.Failure<PagedList<UserResponse>>(new Error("HttpRequestFailure",
                "Не удалось загрузить список пользователей"));
        }
    }

    private HttpClient GetHttpClient() => _httpClient ??= _httpClientFactory.CreateClient("apiBackend");
}