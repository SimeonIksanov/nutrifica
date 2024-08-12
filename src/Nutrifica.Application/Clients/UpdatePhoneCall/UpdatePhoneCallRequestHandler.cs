using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.UpdatePhoneCall;

public class UpdatePhoneCallRequestHandler : ICommandHandler<UpdatePhoneCallCommand, PhoneCallResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientRepository _clientRepository;

    public UpdatePhoneCallRequestHandler(IUnitOfWork unitOfWork, IClientRepository clientRepository)
    {
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
    }

    public async Task<Result<PhoneCallResponse>> Handle(UpdatePhoneCallCommand command,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken);
        if (client is null)
            return Result.Failure<PhoneCallResponse>(ClientErrors.ClientNotFound);
        var phoneCall = client.PhoneCalls.FirstOrDefault(x => x.Id == command.PhoneCallId.Value);
        if (phoneCall is null)
            return Result.Failure<PhoneCallResponse>(ClientErrors.PhoneCallNotFound);

        phoneCall.Comment = command.Comment;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new PhoneCallResponse()
        {
            Id = phoneCall.Id,
            Comment = phoneCall.Comment,
            CreatedOn = phoneCall.CreatedOn,
            CreatedBy = new UserFullNameResponse(phoneCall.CreatedBy.Value, string.Empty, string.Empty,
                string.Empty)
        });
    }
}