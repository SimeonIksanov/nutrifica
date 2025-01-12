using System.Globalization;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Notifications;
using Nutrifica.Application.Notifications.Create;
using Nutrifica.Application.Notifications.Get;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Api.Controllers;

[Route("api/notifications")]
[ApiController]
public class NotificationsController : ApiController
{
    public NotificationsController(IMediator mediator) : base(mediator) { }

    /// <summary>
    ///     Get Notifications within the specified date range.
    /// </summary>
    /// <param name="since">The start date of the range in ISO 8601 format.</param>
    /// <param name="till">The end date of the range in ISO 8601 format.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Returns a collection of notifications.</returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string since, [FromQuery] string till, CancellationToken ct)
    {
        var query = new GetNotificationsQuery() { Since = ParseDateTimeAsUTC(since), Till = ParseDateTimeAsUTC(till), };
        Result<ICollection<NotificationDto>> result = await _mediator.Send(query, ct);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    ///     Create a new notification.
    /// </summary>
    /// <param name="createDto">The notification data transfer object.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>Returns the result of the creation operation.</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NotificationCreateDto createDto, CancellationToken ct)
    {
        var command = new CreateNotificationCommand()
        {
            Message = createDto.Message, DateTime = createDto.DateTime, RecipientId = createDto.RecipientId
        };
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Created() : HandleFailure(result);
    }

    private DateTime ParseDateTimeAsUTC(string dateTime)
    {
        var parsed = DateTime.ParseExact(dateTime, "s", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
        return DateTime.SpecifyKind(parsed, DateTimeKind.Utc);
    }
}