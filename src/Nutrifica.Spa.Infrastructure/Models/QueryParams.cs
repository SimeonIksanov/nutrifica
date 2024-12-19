namespace Nutrifica.Spa.Infrastructure.Models;

public class QueryParams
{
    public string? Filters { get; set; }
    public string? Sorts { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public override string ToString()
    {
        var queryParamsArray = new[]
        {
            $"pagesize={PageSize}",
            $"page={Page}",
            $"filters={Filters}",
            $"sorts={Sorts}",
        };
        return "?" + string.Join('&', queryParamsArray);;
    }
}