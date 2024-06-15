using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nutrifica.Api.Contracts.Authentication;
using Nutrifica.Application.Authentication.Login;

namespace Nutrifica.Api.Controllers;

[Authorize]
[Route("api/auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly ISender _mediatr;

    public AuthenticationController(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    /// <summary>
    /// Get Tokens (username, password)
    /// </summary>
    /// <returns>Status 200 OK</returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<TokenResponse>> Login(TokenRequest request, CancellationToken ct)
    {
        var command = new LoginCommand() { Username = request.Username, Password = request.Password };
        var result = await _mediatr.Send(command, ct);
        if (result.IsFailure) return BadRequest();
        return result.Value;
    }

    /// <summary>
    /// Refresh expired JWT 
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPost("refresh")]
    public async Task<ActionResult<TokenResponse>> Refresh(RefreshTokenRequest request, CancellationToken ct)
    {
        return new TokenResponse("jwt_token_new", "refresh_token_new");
    }

    /// <summary>
    /// LogOut
    /// </summary>
    /// <param name="request">Refresh token string</param>
    /// <returns>Status 200 OK</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> LogOut(LogoutRequest request, CancellationToken ct)
    {
        return BadRequest("NotImplemented");
    }
}
