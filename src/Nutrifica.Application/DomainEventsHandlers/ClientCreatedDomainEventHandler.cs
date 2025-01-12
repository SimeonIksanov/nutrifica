using MediatR;

using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate.Events;
using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Application.DomainEventsHandlers;

public class ClientCreatedDomainEventHandler : INotificationHandler<ClientCreatedDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserClientAccessRepository _userClientAccessRepository;

    public ClientCreatedDomainEventHandler(
        IUnitOfWork unitOfWork, IUserClientAccessRepository userClientAccessRepository)
    {
        _unitOfWork = unitOfWork;
        _userClientAccessRepository = userClientAccessRepository;
    }

    public async Task Handle(ClientCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var access = new UserClientAccess
        {
            UserId = domainEvent.UserId,
            ClientId = domainEvent.ClientId,
            AccessLevel = UserClientAccessLevel.Full
        };
        _userClientAccessRepository.Add(access);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}