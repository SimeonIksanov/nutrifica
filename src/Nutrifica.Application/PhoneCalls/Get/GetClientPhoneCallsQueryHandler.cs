using Microsoft.Extensions.Logging;

using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.PhoneCalls.Get;

public class GetClientPhoneCallsQueryHandler : IQueryHandler<GetClientPhoneCallsQuery, PagedList<PhoneCallDto>>
{
    private readonly IClientRepository _clientRepository;
    private readonly IPhoneCallRepository _phoneCallRepository;
    private readonly ILogger<GetClientPhoneCallsQueryHandler> _logger;

    public GetClientPhoneCallsQueryHandler(IClientRepository clientRepository,
        IPhoneCallRepository phoneCallRepository,
        ILogger<GetClientPhoneCallsQueryHandler> logger)
    {
        _clientRepository = clientRepository;
        _phoneCallRepository = phoneCallRepository;
        _logger = logger;
    }


    public async Task<Result<PagedList<PhoneCallDto>>> Handle(GetClientPhoneCallsQuery request,
        CancellationToken cancellationToken)
    {
        var clientExists = await _clientRepository
            .HasClientWithIdAsync(request.Id, cancellationToken);
        if (!clientExists)
        {
            _logger.LogWarning("Client '{clientId}' not found", request.Id);
            return Result.Failure<PagedList<PhoneCallDto>>(ClientErrors.ClientNotFound);
        }

        var callsPagedList = await _phoneCallRepository
            .GetByFilterAsync(request.Id, request.QueryParams, cancellationToken);

        return Result.Success(PagedList<PhoneCallDto>.Create(
            items: callsPagedList.Items.Select(x => x.ToPhoneCallDto()).ToList(),
            page: callsPagedList.Page,
            pageSize: callsPagedList.PageSize,
            totalCount: callsPagedList.TotalCount));
    }
}