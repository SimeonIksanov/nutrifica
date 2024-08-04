using System.Net.Http.Json;

using Nutrifica.Shared.Wrappers;
using Nutrifica.Spa.Infrastructure.Models;

namespace Nutrifica.Spa.Infrastructure.Services;

public abstract class ServiceBase
{
    protected readonly IHttpClientFactory _httpClientFactory;
    protected HttpClient? _httpClient;

    protected ServiceBase(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected Error ErrorFrom(ProblemDetails? problemDetails)
    {
        if (problemDetails is null)
            return Error.NullValue;
        return problemDetails.Errors is null || problemDetails.Errors.Count == 0
            ? new Error(string.Empty, problemDetails.Detail)
            : new Error(problemDetails.Errors.First().Code, problemDetails.Errors.First().Description);
    }

    protected async Task<T?> ParseResponseAsync<T>(HttpResponseMessage responseMessage,
        CancellationToken cancellationToken)
    {
        return await responseMessage.Content.ReadFromJsonAsync<T>(cancellationToken);
    }

    protected HttpClient GetHttpClient() => _httpClient ??= _httpClientFactory.CreateClient("apiBackend");

    protected async Task<IResult<T>> HandleResponse<T>(HttpResponseMessage httpResponseMessage,
        CancellationToken cancellationToken)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var clientResponse = await ParseResponseAsync<T>(httpResponseMessage, cancellationToken);
            return Result.Success(clientResponse)!;
        }

        var problemDetails = await ParseResponseAsync<ProblemDetails>(httpResponseMessage, cancellationToken);
        return Result.Failure<T>(ErrorFrom(problemDetails));
    }

    protected async Task<IResult> HandleResponse(HttpResponseMessage httpResponseMessage,
        CancellationToken cancellationToken)
    {
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return Result.Success();
        }

        var problemDetails = await ParseResponseAsync<ProblemDetails>(httpResponseMessage, cancellationToken);
        return Result.Failure(ErrorFrom(problemDetails));
    }
}