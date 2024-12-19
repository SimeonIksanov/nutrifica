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
}