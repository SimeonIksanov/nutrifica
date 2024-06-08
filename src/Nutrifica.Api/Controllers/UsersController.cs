using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nutrifica.Api.Contracts.Users;
using Nutrifica.Application.CommandAndQueries.Users;

namespace Nutrifica.Api.Controllers;

[Authorize]
[Route("api/users")]
[ApiController]
public class UsersController : ApiController
{
    public UsersController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Get Users
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var command = new UserCreateCommand
        {
            Username = "asdf",
            FirstName = "sdfasdfsdasf"
        };
        var result = await _mediator.Send(command, ct);
        if (result.Failure)
        {
            return HandleFailure(result);
        }
        return Ok(Array.Empty<UserDTO>());
    }

    
}