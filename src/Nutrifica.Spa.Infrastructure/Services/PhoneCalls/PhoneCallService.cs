using System.Net.Http.Json;

using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;
using Nutrifica.Spa.Infrastructure.Routes;
using Nutrifica.Spa.Infrastructure.Services.Clients;

namespace Nutrifica.Spa.Infrastructure.Services.PhoneCalls;

public class PhoneCallService : ServiceBase, IPhoneCallService
{
    public PhoneCallService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IResult<PagedList<PhoneCallDto>>> GetPhoneCallsAsync(Guid clientId, QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        try
        {
            var requestUri = PhoneCallEndpoints.GetPhoneCalls(clientId) + queryParams;
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

    public async Task<IResult<PhoneCallDto>> CreatePhoneCallAsync(Guid clientId, PhoneCallCreateDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .PostAsJsonAsync(PhoneCallEndpoints.CreatePhoneCall(clientId), dto, cancellationToken);
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
                .PutAsJsonAsync(PhoneCallEndpoints.UpdatePhoneCall(clientId, dto.Id), dto, cancellationToken);
            return await HandleResponse<PhoneCallDto>(response, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex);
            return Result.Failure<PhoneCallDto>(ClientServiceErrors.FailedToUpdatePhoneCalls);
        }
    }

    public async Task<IResult> DeletePhoneCallAsync(Guid clientId, Guid phoneCallId, CancellationToken cancellationToken)
    {
        try
        {
            var response = await GetHttpClient()
                .DeleteAsync(PhoneCallEndpoints.DeletePhoneCall(clientId, phoneCallId), cancellationToken);
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