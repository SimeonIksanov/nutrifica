using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Application.PhoneCalls.Create;
using Nutrifica.Application.PhoneCalls.Get;
using Nutrifica.Application.PhoneCalls.Update;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate.ValueObjects;

namespace Nutrifica.Api.Controllers;

[Route("api/phonecalls")]
[ApiController]
public class PhoneCallsController : ApiController
{
    public PhoneCallsController(IMediator mediator) : base(mediator)
    {
    }

    #region PhoneCalls

    /// <summary>
    /// Get client phonecalls
    /// </summary>
    /// <param name="clientId">Client ID</param>
    /// <param name="queryParams">Filter options</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Returns phonecalls</returns>
    [HttpGet("client/{clientId:guid}")]
    public async Task<IActionResult> GetClientPhoneCalls([FromRoute] Guid clientId, [FromQuery] QueryParams queryParams,
        CancellationToken ct)
    {
        var query = new GetClientPhoneCallsQuery(ClientId.Create(clientId), queryParams);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    /// Create client phonecall
    /// </summary>
    /// <param name="clientId">Client ID</param>
    /// <param name="dto">Create model</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Created phonecall object</returns>
    [HttpPost("client/{clientId:guid}")]
    public async Task<IActionResult> CreateClientPhoneCall([FromRoute] Guid clientId,
        [FromBody] PhoneCallCreateDto dto,
        CancellationToken ct)
    {
        var query = new CreatePhoneCallCommand(ClientId.Create(clientId), dto.Comment);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? StatusCode(StatusCodes.Status201Created, result.Value) : HandleFailure(result);
    }

    /// <summary>
    /// Updates client phonecall
    /// </summary>
    /// <param name="clientId">Client ID</param>
    /// <param name="phoneCallId">Phone call ID</param>
    /// <param name="dto">Update model</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Created client object</returns>
    [HttpPut("client/{clientId:guid}/{phoneCallId:guid}")]
    public async Task<IActionResult> UpdatePhoneCall([FromRoute] Guid clientId, [FromRoute] Guid phoneCallId,
        [FromBody] PhoneCallUpdateDto dto,
        CancellationToken ct)
    {
        if (phoneCallId != dto.Id) return BadRequest();
        var command =
            new UpdatePhoneCallCommand(ClientId.Create(clientId), PhoneCallId.Create(dto.Id), dto.Comment);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // /// <summary>
    // /// Deletes phonecall
    // /// </summary>
    // /// <param name="clientId">Client ID</param>
    // /// <param name="phoneCallId">Phone call ID</param>
    // /// <param name="ct">Cancellation Token</param>
    // /// <returns>Created client object</returns>
    // [HttpDelete("client/{clientId:guid}/{phoneCallId:guid}")]
    // public async Task<IActionResult> DeletePhoneCall([FromRoute] Guid clientId, [FromRoute] Guid phoneCallId,
    //     CancellationToken ct)
    // {
    //     var command = new DeletePhoneCallCommand(ClientId.Create(clientId), PhoneCallId.Create(phoneCallId));
    //     var result = await _mediator.Send(command, ct);
    //     return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    // }

    #endregion
}