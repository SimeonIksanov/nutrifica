using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Api.Contracts.Orders;
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

    #region Client

    /// <summary>
    ///     Get Clients
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryParams queryParams, CancellationToken ct)
    {
        var command = new GetClientsQuery(queryParams);
        Result<PagedList<ClientDto>> result = await _mediator.Send(command, ct);

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
    /// <param name="dto"></param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Created client object</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClientCreateDto dto, CancellationToken ct)
    {
        var command = new CreateClientCommand(
            FirstName.Create(dto.FirstName),
            MiddleName.Create(dto.MiddleName),
            LastName.Create(dto.LastName),
            new Address
            {
                City = dto.Address.City,
                Country = dto.Address.Country,
                Region = dto.Address.Region,
                ZipCode = dto.Address.ZipCode,
                Comment = dto.Address.Comment,
                Street = dto.Address.Street
            },
            Comment.Create(dto.Comment),
            PhoneNumber.Create(dto.PhoneNumber),
            dto.Source //,
            // UserId.Create(CurrentUserId)
        );
        Result<ClientDto> result = await _mediator.Send(command, ct);
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
    public async Task<IActionResult> Update([FromRoute] Guid clientId, [FromBody] ClientUpdateDto dto,
        CancellationToken ct)
    {
        if (clientId != dto.Id) return BadRequest();
        var command = new UpdateClientCommand(
            ClientId.Create(dto.Id),
            FirstName.Create(dto.FirstName),
            MiddleName.Create(dto.MiddleName),
            LastName.Create(dto.LastName),
            dto.Address.ToAddress(),
            Comment.Create(dto.Comment),
            PhoneNumber.Create(dto.PhoneNumber),
            dto.Source,
            dto.State);
        Result<ClientDto> result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }

    #endregion

    #region PhoneCalls

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
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Created client object</returns>
    [HttpPut("{clientId:guid}/phonecalls/{phoneCallId:int}")]
    public async Task<IActionResult> UpdatePhoneCall([FromRoute] Guid clientId, [FromRoute] int phoneCallId,
        [FromBody] PhoneCallUpdateDto dto,
        CancellationToken ct)
    {
        if (phoneCallId != dto.Id) return BadRequest();
        var command =
            new UpdatePhoneCallCommand(ClientId.Create(clientId), PhoneCallId.Create(dto.Id), dto.Comment);
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

    #endregion

    #region Orders

    [HttpGet("{clientId:guid}/orders/")]
    public async Task<IActionResult> GetOrders([FromRoute] Guid clientId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{clientId:guid}/orders/")]
    public async Task<IActionResult> CreateClientOrder([FromRoute] Guid clientId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{clientId:guid}/orders/{orderId:guid}")]
    public async Task<IActionResult> UpdateClientOrder([FromRoute] Guid clientId, [FromRoute] Guid orderId,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{clientId:guid}/orders/{orderId:guid}")]
    public async Task<IActionResult> DeleteClientOrder([FromRoute] Guid clientId, [FromRoute] Guid orderId,
        CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    #endregion
}