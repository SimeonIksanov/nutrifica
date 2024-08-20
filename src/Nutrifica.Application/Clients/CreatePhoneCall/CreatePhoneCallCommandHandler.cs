using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.ClientAggregate.Entities;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Clients.CreatePhoneCall;

public class CreatePhoneCallCommandHandler : ICommandHandler<CreatePhoneCallCommand, PhoneCallDto>
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePhoneCallCommandHandler(IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PhoneCallDto>> Handle(CreatePhoneCallCommand request,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetByIdAsync(request.clientId, cancellationToken);
        if (client is null)
            return Result.Failure<PhoneCallDto>(ClientErrors.ClientNotFound);
        var newPhoneCall = PhoneCall.Create(request.Comment);
        client.AddPhoneCall(newPhoneCall);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new PhoneCallDto()
        {
            Id = newPhoneCall.Id,
            Comment = newPhoneCall.Comment,
            CreatedOn = newPhoneCall.CreatedOn,
            CreatedBy = new UserShortDto(newPhoneCall.CreatedBy.Value, string.Empty, string.Empty,
                string.Empty)
        });
    }
}