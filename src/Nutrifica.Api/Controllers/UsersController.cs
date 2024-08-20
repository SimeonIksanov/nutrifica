using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Users.Requests;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Shared;
using Nutrifica.Application.Users.ChangePassword;
using Nutrifica.Application.Users.Create;
using Nutrifica.Application.Users.Get;
using Nutrifica.Application.Users.GetById;
using Nutrifica.Application.Users.ResetPassword;
using Nutrifica.Application.Users.Update;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ApiController
{
    public UsersController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    ///     Get Users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryParams queryParams, CancellationToken ct)
    {
        var command = new GetUsersQuery(queryParams);
        Result<PagedList<UserDto>> result = await _mediator.Send(command, ct);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    ///     Get User by Id
    /// </summary>
    /// <param name="id">User Identificator</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var command = new GetUserQuery(UserId.Create(id));
        Result<UserDto> result = await _mediator.Send(command, ct);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    ///     Create new User
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto dto, CancellationToken ct)
    {
        var command = new CreateUserCommand(
            dto.Username,
            FirstName.Create(dto.FirstName),
            MiddleName.Create(dto.MiddleName),
            LastName.Create(dto.LastName),
            Email.Create(dto.Email),
            PhoneNumber.Create(dto.PhoneNumber),
            dto.SupervisorId.HasValue ? UserId.Create(dto.SupervisorId.Value) : null);
        Result<UserDto> result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : HandleFailure(result);
    }

    /// <summary>
    ///     Update user's data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateDto dto,
        CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest();

        var command = new UpdateUserCommand(
            UserId.Create(dto.Id),
            dto.Username,
            FirstName.Create(dto.FirstName),
            MiddleName.Create(dto.MiddleName),
            LastName.Create(dto.LastName),
            Email.Create(dto.Email),
            PhoneNumber.Create(dto.PhoneNumber),
            dto.Role,
            dto.Enabled,
            dto.DisableReason,
            dto.SupervisorId.HasValue ? UserId.Create(dto.SupervisorId.Value) : null);
        Result<UserDto> result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    ///     Change user's password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("{id:guid}/changePassword")]
    public async Task<IActionResult> ChangePassword([FromRoute] Guid id, [FromBody] UserChangePasswordDto dto,
        CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest();
        var command = new ChangePasswordCommand(UserId.Create(id), dto.CurrentPassword, dto.NewPassword);
        Result result = await _mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }

    /// <summary>
    ///     Reset user's password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("{id:guid}/resetPassword")]
    public async Task<IActionResult> ResetPassword([FromRoute] Guid id, [FromBody] UserResetPasswordDto dto,
        CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest();
        var command = new ResetPasswordCommand(UserId.Create(id), dto.NewPassword);
        Result result = await _mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }
}