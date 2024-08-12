using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Clients.Create;
using Nutrifica.Application.Clients.CreatePhoneCall;
using Nutrifica.Application.Clients.DeletePhoneCall;
using Nutrifica.Application.Clients.Get;
using Nutrifica.Application.Clients.GetById;
using Nutrifica.Application.Clients.GetClientPhoneCalls;
using Nutrifica.Application.Clients.Update;
using Nutrifica.Application.Clients.UpdatePhoneCall;
using Nutrifica.Application.Mappings;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Api.Controllers;

[Route("api/clients")]
[ApiController]
public class ClientController : ApiController
{
    public ClientController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    ///     Get Clients
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryParams queryParams, CancellationToken ct)
    {
        var command = new GetClientsQuery(queryParams);
        Result<PagedList<ClientResponse>> result = await _mediator.Send(command, ct);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    /// Get client info
    /// </summary>
    /// <param name="clientId"></param>
    /// <returns></returns>
    [HttpGet("{clientId:guid}")]
    public async Task<IActionResult> GetClient([FromRoute] Guid clientId, CancellationToken ct)
    {
        var query = new GetClientQuery(ClientId.Create(clientId));
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    ///     Create client
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Created client object</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClientCreateRequest request, CancellationToken ct)
    {
        var command = new CreateClientCommand(
            FirstName.Create(request.FirstName),
            MiddleName.Create(request.MiddleName),
            LastName.Create(request.LastName),
            new Address
            {
                City = request.Address.City,
                Country = request.Address.Country,
                Region = request.Address.Region,
                ZipCode = request.Address.ZipCode,
                Comment = request.Address.Comment,
                Street = request.Address.Street
            },
            Comment.Create(request.Comment),
            PhoneNumber.Create(request.PhoneNumber),
            request.Source //,
            // UserId.Create(CurrentUserId)
        );
        Result<ClientResponse> result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetClient), new { clientId = result.Value.Id }, result.Value)
            : HandleFailure(result);
    }

    /// <summary>
    ///     Update client data
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPut("{clientId:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid clientId, [FromBody] ClientUpdateRequest request,
        CancellationToken ct)
    {
        if (clientId != request.Id) return BadRequest();
        var command = new UpdateClientCommand(
            ClientId.Create(request.Id),
            FirstName.Create(request.FirstName),
            MiddleName.Create(request.MiddleName),
            LastName.Create(request.LastName),
            request.Address.ToAddress(),
            Comment.Create(request.Comment),
            PhoneNumber.Create(request.PhoneNumber),
            request.Source,
            request.State);
        Result<ClientResponse> result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }


    /// <summary>
    /// Get client phonecalls
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Returns phonecalls</returns>
    [HttpGet("{clientId:guid}/phonecalls")]
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
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Created phonecall object</returns>
    [HttpPost("{clientId:guid}/phonecalls")]
    public async Task<IActionResult> CreateClientPhoneCall([FromRoute] Guid clientId,
        [FromBody] PhoneCallCreateRequest request,
        CancellationToken ct)
    {
        var query = new CreatePhoneCallCommand(ClientId.Create(clientId), request.Comment);
        var result = await _mediator.Send(query, ct);
        return result.IsSuccess ? StatusCode(StatusCodes.Status201Created, result.Value) : HandleFailure(result);
    }

    /// <summary>
    /// Updates client phonecall
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Created client object</returns>
    [HttpPut("{clientId:guid}/phonecalls/{phoneCallId:int}")]
    public async Task<IActionResult> UpdatePhoneCall([FromRoute] Guid clientId, [FromRoute] int phoneCallId,
        [FromBody] PhoneCallUpdateRequest request,
        CancellationToken ct)
    {
        if (phoneCallId != request.Id) return BadRequest();
        var command = new UpdatePhoneCallCommand(ClientId.Create(clientId), PhoneCallId.Create(request.Id), request.Comment);
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Deletes phonecall
    /// </summary>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Created client object</returns>
    [HttpDelete("{clientId:guid}/phonecalls/{phoneCallId:int}")]
    public async Task<IActionResult> DeletePhoneCall([FromRoute] Guid clientId, [FromRoute] int phoneCallId,
        CancellationToken ct)
    {
        var command = new DeletePhoneCallCommand(ClientId.Create(clientId), PhoneCallId.Create(phoneCallId));
        var result = await _mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}