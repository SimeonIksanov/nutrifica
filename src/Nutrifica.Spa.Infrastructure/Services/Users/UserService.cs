using System.Net.Http.Json;

using Nutrifica.Api.Contracts.Users.Requests;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
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
        var requestUri = UsersEndpoints.Get + "?pagesize=50";
        try
        {
            var pagedList = await GetHttpClient()
                .GetFromJsonAsync<PagedList<UserResponse>>(requestUri, cancellationToken);
            return Result.Success(pagedList!);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список пользователей: {ex.Message}");
            return Result.Failure<PagedList<UserResponse>>(UserServiceErrors.FailedToLoad);
        }
    }

    public async Task<IResult<UserResponse>> Create(UserCreateRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(UsersEndpoints.Create, request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var userResponse = await ParseResponse<UserResponse>(response, cancellationToken);
                return Result.Success(userResponse!);
            }

            var problemDetails = await ParseResponse<ProblemDetails>(response, cancellationToken);
            var error = problemDetails.Errors is null
                ? new Error(string.Empty, problemDetails.Detail)
                : new Error(problemDetails.Errors.First().Code, problemDetails.Errors.First().Description);
            return Result.Failure<UserResponse>(error);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<UserResponse>(UserServiceErrors.FailedToCreate);
        }
    }

    private async Task<T> ParseResponse<T>(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
    {
        return await responseMessage.Content.ReadFromJsonAsync<T>(cancellationToken);
    }

    private HttpClient GetHttpClient() => _httpClient ??= _httpClientFactory.CreateClient("apiBackend");
}