namespace Nutrifica.Application.Shared;

public class QueryParams
{
    public string? Filters { get; set; }
    public string? Sorts { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}