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

    public async Task<IResult<PagedList<UserDto>>> GetAsync(QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        var requestUri = UsersEndpoints.Get + queryParams;
        try
        {
            var response = await GetHttpClient().GetAsync(requestUri, cancellationToken);
            return await HandleResponse<PagedList<UserDto>>(response, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список пользователей: {ex.Message}");
            return Result.Failure<PagedList<UserDto>>(UserServiceErrors.FailedToLoad);
        }
    }

    public async Task<IResult<UserDto>> CreateAsync(UserCreateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(UsersEndpoints.Create, dto, cancellationToken);
            return await HandleResponse<UserDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<UserDto>(UserServiceErrors.FailedToCreate);
        }
    }

    public async Task<IResult<UserDto>> UpdateAsync(UserUpdateDto dto, CancellationToken cancellationToken)
    {
        var uri = UsersEndpoints.Update(dto.Id);
        try
        {
            var response = await GetHttpClient()
                .PutAsJsonAsync(uri, dto, cancellationToken);
            return await HandleResponse<UserDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<UserDto>(UserServiceErrors.FailedToUpdate);
        }
    }

    public async Task<IResult> ChangePasswordAsync(UserChangePasswordDto dto,
        CancellationToken cancellationToken)
    {
        var uri = UsersEndpoints.ChangePassword(dto.Id);
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(uri, dto, cancellationToken);
            return await HandleResponse(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine(ex);
            return Result.Failure(UserServiceErrors.FailedToChangePassword);
        }
    }

    public async Task<IResult> ResetPasswordAsync(UserResetPasswordDto dto, CancellationToken cancellationToken)
    {
        var uri = UsersEndpoints.ResetPassword(dto.Id);
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(uri, dto, cancellationToken);
            return await HandleResponse(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.Error.WriteLine(ex);
            return Result.Failure(UserServiceErrors.FailedToResetPassword);
        }
    }

    public async Task<IResult<ICollection<UserShortDto>>> GetManagersAsync(CancellationToken cancellationToken)
    {
        var requestUri = UsersEndpoints.GetManagers;
        try
        {
            var response = await GetHttpClient().GetAsync(requestUri, cancellationToken);
            return await HandleResponse<ICollection<UserShortDto>>(response, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список пользователей: {ex.Message}");
            return Result.Failure<ICollection<UserShortDto>>(UserServiceErrors.FailedToLoad);
        }
    }
}