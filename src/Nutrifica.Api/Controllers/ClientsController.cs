using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Api.Contracts.PhoneCalls;
using Nutrifica.Application.Clients.Create;
using Nutrifica.Application.Clients.Get;
using Nutrifica.Application.Clients.GetById;
using Nutrifica.Application.Clients.Update;
using Nutrifica.Application.Mappings;
using Nutrifica.Application.PhoneCalls.Get;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.PhoneCallAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Api.Controllers;

[Route("api/clients")]
[ApiController]
public class ClientsController : ApiController
{
    public ClientsController(IMediator mediator) : base(mediator)
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
        var command = new UpdateClientCommand
        {
            Id = ClientId.Create(dto.Id),
            FirstName = FirstName.Create(dto.FirstName),
            MiddleName = MiddleName.Create(dto.MiddleName),
            LastName = LastName.Create(dto.LastName),
            Address = dto.Address.ToAddress(),
            Comment = Comment.Create(dto.Comment),
            PhoneNumber = PhoneNumber.Create(dto.PhoneNumber),
            Source = dto.Source,
            State = dto.State,
            ManagerIds = dto.ManagerIds.Select(UserId.Create).ToArray()
        };
        Result<ClientDto> result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }

    #endregion
}