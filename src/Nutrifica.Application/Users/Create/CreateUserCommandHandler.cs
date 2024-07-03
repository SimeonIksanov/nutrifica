using Nutrifica.Api.Contracts.Users;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Mappings;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Users.Create;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await UsernameIsUsed(request.Username, cancellationToken))
            return Result.Failure<UserResponse>(UserErrors.UsernameIsAlreadyInUse);

        User? supervisor = null;
        if (request.SupervisorId is not null && Guid.Empty != request.SupervisorId.Value)
        {
            supervisor = await _userRepository.GetByIdAsync(request.SupervisorId, cancellationToken);
            if (supervisor is null)
            {
                return Result.Failure<UserResponse>(UserErrors.SupervisorNotFound);
            }
        }

        var user = User.Create(request.Username, request.FirstName, request.MiddleName, request.LastName,
            request.PhoneNumber, request.Email, request.SupervisorId);

        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.ToUserResponse(supervisor);
    }

    private async Task<bool> UsernameIsUsed(string username, CancellationToken cancellationToken)
    {
        return await _userRepository.GetCountByUsernameAsync(username, cancellationToken) > 0;
    }
}