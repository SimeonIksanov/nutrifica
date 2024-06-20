using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Users;
using Nutrifica.Application.Users.Get;
using Nutrifica.Shared.QueryParameters;

namespace Nutrifica.Api.Controllers;

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
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var command = new GetUsersQuery(new UserQueryParams());
        var result = await _mediator.Send(command, ct);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    ///     Create new User
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("{id:guid}/changePassword")]
    public async Task<IActionResult> ChangePassword(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("{id:guid}/resetPassword")]
    public async Task<IActionResult> ResetPassword(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}