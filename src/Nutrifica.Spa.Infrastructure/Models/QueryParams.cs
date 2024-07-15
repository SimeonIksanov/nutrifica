namespace Nutrifica.Spa.Infrastructure.Models;

public class QueryParams
{
    public string? Filters { get; set; }
    public string? Sorts { get; set; }
    public int Page { get; set; } = 0;
    public int PageSize { get; set; } = 20;
}