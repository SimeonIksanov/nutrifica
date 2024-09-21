using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.DeletePhoneCall;

public record DeletePhoneCallCommand(ClientId ClientId, PhoneCallId PhoneCallId) : ICommand;

public class DeletePhoneCallCommandHandler : ICommandHandler<DeletePhoneCallCommand>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePhoneCallCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeletePhoneCallCommand request, CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetEntityByIdAsync(request.ClientId, cancellationToken);
        if (client is null)
            return Result.Failure(ClientErrors.ClientNotFound);
        var phoneCall = client.PhoneCalls.FirstOrDefault(x => x.Id == request.PhoneCallId.Value);
        if (phoneCall is null)
            return Result.Failure(ClientErrors.PhoneCallNotFound);
        client.DeletePhoneCall(phoneCall);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}