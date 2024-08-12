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

    public async Task<IResult<PagedList<ClientResponse>>> GetAsync(QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        var requestUri = ClientsEndpoints.Get + queryParams;
        try
        {
            var response = await GetHttpClient().GetAsync(requestUri, cancellationToken);
            return await HandleResponse<PagedList<ClientResponse>>(response, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список клиентов: {ex.Message}");
            return Result.Failure<PagedList<ClientResponse>>(ClientServiceErrors.FailedToLoad);
        }
    }

    public async Task<IResult<ClientResponse>> GetByIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .GetAsync(ClientsEndpoints.GetById(clientId), cancellationToken);
            return await HandleResponse<ClientResponse>(response, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result.Failure<ClientResponse>(ClientServiceErrors.FailedToLoad);
        }
    }

    public async Task<IResult<PagedList<PhoneCallResponse>>> GetPhoneCallsAsync(Guid clientId, QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        try
        {
            var requestUri = ClientsEndpoints.GetPhoneCalls(clientId) + queryParams;
            var response = await GetHttpClient()
                .GetAsync(requestUri, cancellationToken);
            return await HandleResponse<PagedList<PhoneCallResponse>>(response, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось загрузить список пользователей: {ex.Message}");
            return Result.Failure<PagedList<PhoneCallResponse>>(ClientServiceErrors.FailedToLoadPhoneCalls);
        }
    }

    public async Task<IResult<ClientResponse>> CreateAsync(ClientCreateRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(ClientsEndpoints.Create, request, cancellationToken);
            return await HandleResponse<ClientResponse>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<ClientResponse>(ClientServiceErrors.FailedToCreate);
        }
    }

    public async Task<IResult<ClientResponse>> UpdateAsync(ClientUpdateRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PutAsJsonAsync(ClientsEndpoints.Update(request.Id), request, cancellationToken);
            return await HandleResponse<ClientResponse>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<ClientResponse>(ClientServiceErrors.FailedToUpdate);
        }
    }

    public async Task<IResult<PhoneCallResponse>> CreatePhoneCallAsync(Guid clientId, PhoneCallCreateRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(ClientsEndpoints.CreatePhoneCall(clientId), request, cancellationToken);
            return await HandleResponse<PhoneCallResponse>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<PhoneCallResponse>(ClientServiceErrors.FailedToCreatePhoneCall);
        }
    }

    public async Task<IResult<PhoneCallResponse>> UpdatePhoneCallAsync(Guid clientId, PhoneCallUpdateRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PutAsJsonAsync(ClientsEndpoints.UpdatePhoneCall(clientId, request.Id), request, cancellationToken);
            return await HandleResponse<PhoneCallResponse>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<PhoneCallResponse>(ClientServiceErrors.FailedToUpdatePhoneCalls);
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