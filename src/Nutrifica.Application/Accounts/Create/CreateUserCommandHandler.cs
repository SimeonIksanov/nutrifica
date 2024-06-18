using Nutrifica.Api.Contracts.Users;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Domain;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Accounts.Create;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserDTO>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDTO>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User? supervisor = null;
        if (request.SupervisorId is not null && Guid.Empty != request.SupervisorId.Value)
        {
            supervisor = await _userRepository.GetByIdAsync(request.SupervisorId, cancellationToken);
            if (supervisor is null)
            {
                return Result.Failure<UserDTO>(UserErrors.SupervisorNotFound);
            }
        }

        var user = User.Create(request.Username, request.FirstName, request.MiddleName, request.LastName,
            request.PhoneNumber, request.Email, request.SupervisorId);

        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: May be AutoMapper should be applied?
        var userDto = new UserDTO(
            user.Id.Value,
            user.Account.Username,
            user.FirstName.Value,
            user.MiddleName.Value,
            user.LastName.Value,
            user.Email.Value,
            user.PhoneNumber.Value,
            user.Enabled,
            user.DisableReason,
            supervisor?.Id.Value ?? Guid.Empty,
            supervisor?.FullName ?? string.Empty,
            user.Role,
            user.CreatedAt);
        return userDto;
    }
}