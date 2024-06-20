using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Users.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasherService;

    public ChangePasswordCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        IPasswordHasherService passwordHasherService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdIncludeAccountAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.UserNotFound);
        }

        // TODO: Permissions required here
        if (string.IsNullOrWhiteSpace(request.CurrentPassword) ||
            !_passwordHasherService.Verify(request.CurrentPassword, user.Account.PasswordHash, user.Account.Salt))
        {
            return Result.Failure(UserErrors.WrongPassword);
        }

        var pair = _passwordHasherService.HashPassword(request.NewPassword);
        user.Account.PasswordHash = pair.hashed;
        user.Account.Salt = pair.salt;
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}