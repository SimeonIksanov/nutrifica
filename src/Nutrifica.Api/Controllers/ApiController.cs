using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nutrifica.Shared;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Api.Controllers;

[Authorize]
[ApiController]
public class ApiController : ControllerBase
{
    protected readonly IMediator _mediator;

    public ApiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected IActionResult HandleFailure(Result result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult => BadRequest(CreateProblemDetails("Validation Error",
                StatusCodes.Status400BadRequest, result.Error.Description, validationResult.Errors)),
            _ => BadRequest(CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, result.Error.Description))
        };
    }

    private static ProblemDetails CreateProblemDetails(string title,
        int status,
        string errorMessage,
        Error[] errors=null)
    {
        return new()
        {
            Title = title,
            //Type =
            Detail = errorMessage,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
    }
}
