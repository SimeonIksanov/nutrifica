using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.ClientAggregate;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.PhoneCalls.Create;

public class CreatePhoneCallCommandHandler : ICommandHandler<CreatePhoneCallCommand, PhoneCallDto>
{
    private readonly IClientRepository _clientRepository;
    private readonly IPhoneCallRepository _phoneCallRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePhoneCallCommandHandler(
        IClientRepository clientRepository, 
        IPhoneCallRepository phoneCallRepository, 
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _phoneCallRepository = phoneCallRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PhoneCallDto>> Handle(CreatePhoneCallCommand request,
        CancellationToken cancellationToken)
    {
        var client = await _clientRepository.GetEntityByIdAsync(request.clientId, cancellationToken);
        if (client is null)
            return Result.Failure<PhoneCallDto>(ClientErrors.ClientNotFound);
        var newPhoneCall = PhoneCall.Create(request.clientId ,request.Comment);
        _phoneCallRepository.Add(newPhoneCall);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new PhoneCallDto()
        {
            Id = newPhoneCall.Id.Value,
            Comment = newPhoneCall.Comment,
            CreatedOn = newPhoneCall.CreatedOn,
            CreatedBy = (await _userRepository.GetShortByIdAsync(newPhoneCall.CreatedBy, cancellationToken))!.ToUserShortDto()
        });
    }
}