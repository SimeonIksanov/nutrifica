using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Users;

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
        return Ok(Array.Empty<UserResponse>());
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