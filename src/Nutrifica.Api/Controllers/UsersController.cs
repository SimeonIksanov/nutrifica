using MediatR;

using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Users.Requests;
using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Users.ChangePassword;
using Nutrifica.Application.Users.Create;
using Nutrifica.Application.Users.Get;
using Nutrifica.Application.Users.GetById;
using Nutrifica.Application.Users.ResetPassword;
using Nutrifica.Application.Users.Update;
using Nutrifica.Domain.Aggregates.ClientAggregate.ValueObjects;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Domain.Shared;
using Nutrifica.Shared.QueryParameters;
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
    public async Task<IActionResult> Get([FromQuery] UserQueryParams queryParams, CancellationToken ct)
    {
        var command = new GetUsersQuery(queryParams);
        Result<IPagedList<UserResponse>> result = await _mediator.Send(command, ct);

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
        Result<UserResponse> result = await _mediator.Send(command, ct);

        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    ///     Create new User
    /// </summary>
    /// <param name="request"></param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateRequest request, CancellationToken ct)
    {
        var command = new CreateUserCommand(
            request.Username,
            FirstName.Create(request.FirstName),
            MiddleName.Create(request.MiddleName),
            LastName.Create(request.LastName),
            Email.Create(request.Email),
            PhoneNumber.Create(request.PhoneNumber),
            request.SupervisorId.HasValue ? UserId.Create(request.SupervisorId.Value) : null);
        Result<UserResponse> result = await _mediator.Send(command, ct);
        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
            : HandleFailure(result);
    }

    /// <summary>
    ///     Update user's data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateRequest request,
        CancellationToken ct)
    {
        if (id != request.Id) return BadRequest();

        var command = new UpdateUserCommand(
            UserId.Create(request.Id),
            request.Username,
            FirstName.Create(request.FirstName),
            MiddleName.Create(request.MiddleName),
            LastName.Create(request.LastName),
            Email.Create(request.Email),
            PhoneNumber.Create(request.PhoneNumber),
            request.Role,
            request.Enabled,
            request.DisableReason,
            request.SupervisorId.HasValue ? UserId.Create(request.SupervisorId.Value) : null);
        Result<UserResponse> result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    ///     Change user's password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("{id:guid}/changePassword")]
    public async Task<IActionResult> ChangePassword([FromRoute] Guid id, [FromBody] UserChangePasswordRequest request,
        CancellationToken ct)
    {
        var command = new ChangePasswordCommand(UserId.Create(id), request.CurrentPassword, request.NewPassword);
        Result result = await _mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }

    /// <summary>
    ///     Reset user's password
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpPost("{id:guid}/resetPassword")]
    public async Task<IActionResult> ResetPassword([FromRoute] Guid id, [FromBody] UserResetPasswordRequest request,
        CancellationToken ct)
    {
        var command = new ResetPasswordCommand(UserId.Create(id), request.NewPassword);
        Result result = await _mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }
}