namespace Nutrifica.Shared.QueryParameters;

public abstract record QueryParams(
    string SortColumn = "id",
    string SortOrder = "asc",
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null);

public record UserQueryParams(string SortColumn = "name") : QueryParams;
public record ClientQueryParams(string SortColumn = "name") : QueryParams;
    