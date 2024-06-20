using Nutrifica.Api.Contracts.Users;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Users.Update;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? supervisor = null;
        if (request.SupervisorId is not null && Guid.Empty != request.SupervisorId.Value)
        {
            supervisor = await _userRepository.GetByIdAsync(request.SupervisorId, cancellationToken);
            if (supervisor is null)
            {
                return Result.Failure<UserResponse>(UserErrors.SupervisorNotFound);
            }
        }

        if (request.Enabled is false && string.IsNullOrWhiteSpace(request.DisableReason))
        {
            return Result.Failure<UserResponse>(UserErrors.DisableReasonNotSpecified);
        }

        var user = await _userRepository.GetByIdIncludeAccountAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);
        }

        user.FirstName = request.FirstName;
        user.MiddleName = request.MiddleName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.Account.Username = request.Username;
        user.Role = request.Role;
        user.PhoneNumber = request.PhoneNumber;
        user.SupervisorId = request.SupervisorId;
        if (user.Enabled && request.Enabled is false)
        {
            user.Disable(request.DisableReason);
        }

        if (user.Enabled is false && request.Enabled)
        {
            user.Enable();
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.ToUserResponse(supervisor);
    }
}