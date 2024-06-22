using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Spa.Infrastructure.Models;

public class ProblemDetails
{
    public string Type { get; set; }
    public string Title { get; set; }
    public int Status { get; set; }
    public string Detail { get; set; }
    public ICollection<Error> Errors { get; set; }
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