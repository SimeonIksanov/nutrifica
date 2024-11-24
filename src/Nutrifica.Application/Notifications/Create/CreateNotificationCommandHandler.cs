using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Abstractions;
using Nutrifica.Domain.Aggregates.NotificationAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Notifications.Create;

public class CreateNotificationCommandHandler : ICommandHandler<CreateNotificationCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;

    public CreateNotificationCommandHandler(INotificationRepository notificationRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var recipientId = request.RecipientId.HasValue
            ? UserId.Create(request.RecipientId.Value)
            : _currentUserService.UserId;
        var user = await _userRepository.GetByIdAsync(recipientId, cancellationToken);
        if (user is null)
            return Result.Failure<Guid>(UserErrors.UserNotFound);
        if (!user.Enabled)
            return Result.Failure<Guid>(UserErrors.Disabled);
        var notification = Notification.Create(request.DateTime, request.Message, recipientId);
        _notificationRepository.Add(notification);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(notification.Id.Value);
    }
}