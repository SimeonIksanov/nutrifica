using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application;
using Nutrifica.Application.Authentication.Login;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Api.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthenticationController : ApiController
{
    public AuthenticationController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    ///     Get Tokens (username, password)
    /// </summary>
    /// <param name="request">Request object</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Status 200 OK</returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login(TokenRequest request, CancellationToken ct)
    {
        var command = new LoginCommand { Username = request.Username, Password = request.Password };
        Result<TokenResponse> result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    ///     Refresh expired JWT
    /// </summary>
    /// <param name="request">Request object</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Status 200 OK</returns>
    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request, CancellationToken ct)
    {
        var command = new RefreshTokensCommand(request.Jwt, request.RefreshToken, GetRemoteIpAddress());
        Result<TokenResponse> result = await _mediator.Send(command, ct);
        return result.IsSuccess ? Ok(result.Value) : HandleFailure(result);
    }

    /// <summary>
    ///     LogOut
    /// </summary>
    /// <param name="request">Request object</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns>Status 204 NoContent</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> LogOut(LogoutRequest request, CancellationToken ct)
    {
        var command = new LogoutCommand(request.Jwt, request.RefreshToken, GetRemoteIpAddress());
        Result result = await _mediator.Send(command, ct);
        return result.IsSuccess ? NoContent() : HandleFailure(result);
    }


    private string GetRemoteIpAddress()
    {
        const string xForwardedFor = "X-Forwarded-For";
        if (Request.Headers.TryGetValue(xForwardedFor, out Microsoft.Extensions.Primitives.StringValues value))
        {
            // return value!;
            return value.FirstOrDefault() ?? string.Empty;
        }

        return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? string.Empty;
    }
}