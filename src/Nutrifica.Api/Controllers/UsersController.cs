using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nutrifica.Api.Contracts.Users;

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
        return Ok(Array.Empty<UserDTO>());
    }


}
