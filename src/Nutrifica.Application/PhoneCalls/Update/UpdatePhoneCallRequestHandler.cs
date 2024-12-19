using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.PhoneCalls.Update;

public class UpdatePhoneCallRequestHandler : ICommandHandler<UpdatePhoneCallCommand, PhoneCallDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClientRepository _clientRepository;
    private readonly IPhoneCallRepository _phoneCallRepository;
    private readonly IUserRepository _userRepository;

    public UpdatePhoneCallRequestHandler(IUnitOfWork unitOfWork,
        IClientRepository clientRepository,
        IPhoneCallRepository phoneCallRepository,
        IUserRepository userRepository)
    {
        _unitOfWork = unitOfWork;
        _clientRepository = clientRepository;
        _phoneCallRepository = phoneCallRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<PhoneCallDto>> Handle(UpdatePhoneCallCommand command,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetEntityByIdAsync(command.ClientId, cancellationToken);
        if (client is null)
            return Result.Failure<PhoneCallDto>(ClientErrors.ClientNotFound);
        var phoneCall = await _phoneCallRepository.GetEntityByIdAsync(command.PhoneCallId, cancellationToken);
        if (phoneCall is null)
            return Result.Failure<PhoneCallDto>(PhoneCallErrors.PhoneCallNotFound);

        phoneCall.Comment = command.Comment;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new PhoneCallDto()
        {
            Id = phoneCall.Id.Value,
            Comment = phoneCall.Comment,
            CreatedOn = phoneCall.CreatedOn,
            CreatedBy = (await _userRepository.GetShortByIdAsync(phoneCall.CreatedBy, cancellationToken))!.ToUserShortDto()
        });
    }
}