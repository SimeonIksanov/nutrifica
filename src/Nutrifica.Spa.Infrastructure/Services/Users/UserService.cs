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

    public async Task<IResult<UserResponse>> CreateAsync(UserCreateRequest request, CancellationToken cancellationToken)
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
            var error = ErrorFrom(problemDetails);
            return Result.Failure<UserResponse>(error);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<UserResponse>(UserServiceErrors.FailedToCreate);
        }
    }

    public async Task<IResult<UserResponse>> UpdateAsync(UserUpdateRequest request, CancellationToken cancellationToken)
    {
        var uri = UsersEndpoints.Update(request.Id);
        try
        {
            var response = await GetHttpClient()
                .PutAsJsonAsync(uri, request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var userResponse = await ParseResponse<UserResponse>(response, cancellationToken);
                return Result.Success(userResponse!);
            }

            var problemDetails = await ParseResponse<ProblemDetails>(response, cancellationToken);
            var error = ErrorFrom(problemDetails);
            return Result.Failure<UserResponse>(error);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<UserResponse>(UserServiceErrors.FailedToUpdate);
        }
    }
    public async Task<IResult> ChangePasswordAsync(UserChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var uri = UsersEndpoints.ChangePassword(request.Id);
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(uri, request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            var problemDetails = await ParseResponse<ProblemDetails>(response, cancellationToken);
            return Result.Failure(ErrorFrom(problemDetails));
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine(ex);
            return Result.Failure(UserServiceErrors.FailedToChangePassword);
        }
    }

    public async Task<IResult> ResetPasswordAsync(UserResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var uri = UsersEndpoints.ResetPassword(request.Id);
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(uri, request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            var problemDetails = await ParseResponse<ProblemDetails>(response, cancellationToken);
            return Result.Failure(ErrorFrom(problemDetails));
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine(ex);
            return Result.Failure(UserServiceErrors.FailedToResetPassword);
        }
    }

    private Error ErrorFrom(ProblemDetails? problemDetails)
    {
        if (problemDetails is null)
            return Error.NullValue;
        return problemDetails.Errors is null || problemDetails.Errors.Count == 0
            ? new Error(string.Empty, problemDetails.Detail)
            : new Error(problemDetails.Errors.First().Code, problemDetails.Errors.First().Description);
    }

    private async Task<T?> ParseResponse<T>(HttpResponseMessage responseMessage, CancellationToken cancellationToken)
    {
        return await responseMessage.Content.ReadFromJsonAsync<T>(cancellationToken);
    }

    private HttpClient GetHttpClient() => _httpClient ??= _httpClientFactory.CreateClient("apiBackend");
}