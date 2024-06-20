using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Users.ResetPassword;

public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherService _passwordHasherService;

    public ResetPasswordCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        IPasswordHasherService passwordHasherService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasherService = passwordHasherService;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdIncludeAccountAsync(request.Id, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.UserNotFound);
        }

        var pair = _passwordHasherService.HashPassword(request.NewPassword);
        user.Account.PasswordHash = pair.hashed;
        user.Account.Salt = pair.salt;
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}