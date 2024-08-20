using System.Net.Http.Json;

using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Routes;

namespace Nutrifica.Spa.Infrastructure.Services.Clients;

public class ClientService : ServiceBase, IClientService
{
    public ClientService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task<IResult<PagedList<ClientDto>>> GetAsync(QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        var requestUri = ClientsEndpoints.Get + queryParams;
        try
        {
            var response = await GetHttpClient().GetAsync(requestUri, cancellationToken);
            return await HandleResponse<PagedList<ClientDto>>(response, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список клиентов: {ex.Message}");
            return Result.Failure<PagedList<ClientDto>>(ClientServiceErrors.FailedToLoad);
        }
    }

    public async Task<IResult<ClientDto>> GetByIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .GetAsync(ClientsEndpoints.GetById(clientId), cancellationToken);
            return await HandleResponse<ClientDto>(response, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Failure<ClientDto>(ClientServiceErrors.FailedToLoad);
        }
    }

    public async Task<IResult<PagedList<PhoneCallDto>>> GetPhoneCallsAsync(Guid clientId, QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        try
        {
            var requestUri = ClientsEndpoints.GetPhoneCalls(clientId) + queryParams;
            var response = await GetHttpClient()
                .GetAsync(requestUri, cancellationToken);
            return await HandleResponse<PagedList<PhoneCallDto>>(response, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список пользователей: {ex.Message}");
            return Result.Failure<PagedList<PhoneCallDto>>(ClientServiceErrors.FailedToLoadPhoneCalls);
        }
    }

    public async Task<IResult<ClientDto>> CreateAsync(ClientCreateDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(ClientsEndpoints.Create, dto, cancellationToken);
            return await HandleResponse<ClientDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<ClientDto>(ClientServiceErrors.FailedToCreate);
        }
    }

    public async Task<IResult<ClientDto>> UpdateAsync(ClientUpdateDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PutAsJsonAsync(ClientsEndpoints.Update(dto.Id), dto, cancellationToken);
            return await HandleResponse<ClientDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<ClientDto>(ClientServiceErrors.FailedToUpdate);
        }
    }

    public async Task<IResult<PhoneCallDto>> CreatePhoneCallAsync(Guid clientId, PhoneCallCreateDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(ClientsEndpoints.CreatePhoneCall(clientId), dto, cancellationToken);
            return await HandleResponse<PhoneCallDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<PhoneCallDto>(ClientServiceErrors.FailedToCreatePhoneCall);
        }
    }

    public async Task<IResult<PhoneCallDto>> UpdatePhoneCallAsync(Guid clientId, PhoneCallUpdateDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PutAsJsonAsync(ClientsEndpoints.UpdatePhoneCall(clientId, dto.Id), dto, cancellationToken);
            return await HandleResponse<PhoneCallDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<PhoneCallDto>(ClientServiceErrors.FailedToUpdatePhoneCalls);
        }
    }

    public async Task<IResult> DeletePhoneCallAsync(Guid clientId, int phoneCallId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .DeleteAsync(ClientsEndpoints.DeletePhoneCall(clientId, phoneCallId), cancellationToken);
            return response.IsSuccessStatusCode
                ? Result.Success()
                : Result.Failure(ClientServiceErrors.FailedToDeletePhoneCalls);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure(ClientServiceErrors.FailedToDeletePhoneCalls);
        }
    }
}