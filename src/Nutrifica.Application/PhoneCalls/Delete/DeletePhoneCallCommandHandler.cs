// using Nutrifica.Application.Abstractions.Messaging;
// using Nutrifica.Application.Interfaces.Services.Persistence;
// using Nutrifica.Domain.Abstractions;
// using Nutrifica.Domain.Aggregates.ClientAggregate;
// using Nutrifica.Shared.Wrappers;
//
// namespace Nutrifica.Application.PhoneCalls.Delete;
//
// public class DeletePhoneCallCommandHandler : ICommandHandler<DeletePhoneCallCommand>
// {
//     private readonly IClientRepository _clientRepository;
//     private readonly IPhoneCallRepository _phoneCallRepository;
//     private readonly IUnitOfWork _unitOfWork;
//
//     public DeletePhoneCallCommandHandler(IClientRepository clientRepository, IPhoneCallRepository phoneCallRepository, IUnitOfWork unitOfWork)
//     {
//         _clientRepository = clientRepository;
//         _phoneCallRepository = phoneCallRepository;
//         _unitOfWork = unitOfWork;
//     }
//
//     public async Task<Result> Handle(DeletePhoneCallCommand request, CancellationToken cancellationToken)
//     {
//         var client = await _clientRepository.GetEntityByIdAsync(request.ClientId, cancellationToken);
//         if (client is null)
//             return Result.Failure(ClientErrors.ClientNotFound);
//         var phoneCall = await _phoneCallRepository.GetEntityByIdAsync(request.PhoneCallId, cancellationToken);
//         if (phoneCall is null)
//             return Result.Failure(PhoneCallErrors.PhoneCallNotFound);
//         client.DeletePhoneCall(phoneCall);
//         await _unitOfWork.SaveChangesAsync(cancellationToken);
//         return Result.Success();
//     }
// }