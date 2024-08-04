using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Models;

public class ProblemDetails
{
    public string Type { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = String.Empty;
    public ICollection<Error>? Errors { get; set; } = null!;
}

/*
{
    "type": "User.BadLoginOrPassword",
    "title": "Bad Request",
    "status": 400,
    "detail": "Bad username or password.",
    "errors": null
}
*/