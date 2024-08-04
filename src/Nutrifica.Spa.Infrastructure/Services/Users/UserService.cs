using System.Net.Http.Json;

using Nutrifica.Api.Contracts.Users.Requests;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Routes;

namespace Nutrifica.Spa.Infrastructure.Services.Users;

public class UserService : ServiceBase, IUserService
{
    public UserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task<IResult<PagedList<UserResponse>>> Get(QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        var requestUri = UsersEndpoints.Get + queryParams;
        try
        {
            var response = await GetHttpClient().GetAsync(requestUri, cancellationToken);
            return await HandleResponse<PagedList<UserResponse>>(response, cancellationToken);
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
            return await HandleResponse<UserResponse>(response, cancellationToken);
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
            return await HandleResponse<UserResponse>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<UserResponse>(UserServiceErrors.FailedToUpdate);
        }
    }

    public async Task<IResult> ChangePasswordAsync(UserChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var uri = UsersEndpoints.ChangePassword(request.Id);
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(uri, request, cancellationToken);
            return await HandleResponse(response, cancellationToken);
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
            return await HandleResponse(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine(ex);
            return Result.Failure(UserServiceErrors.FailedToResetPassword);
        }
    }
}