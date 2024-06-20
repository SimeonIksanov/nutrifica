using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Clients;
using Nutrifica.Application.Clients.Create;
using Nutrifica.Application.Clients.Update;
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
    public async Task<IActionResult> Get(CancellationToken ct) => Ok(Array.Empty<ClientResponse>());

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetClient(Guid userId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Create client
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        throw new NotImplementedException();
        CreateClientCommand command = null!;
        Result<ClientResponse> result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetClient), new { userId = result.Value }, result.Value)
            : HandleFailure(result);
    }

    /// <summary>
    ///     Update client data
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    [HttpPut("{clientId:guid}")]
    public async Task<IActionResult> Update(Guid clientId, CancellationToken ct)
    {
        throw new NotImplementedException();
        UpdateClientCommand command = null!;
        Result<ClientResponse> result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? Ok(result.Value)
            : HandleFailure(result);
    }
}