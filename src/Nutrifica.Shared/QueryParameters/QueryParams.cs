namespace Nutrifica.Shared.QueryParameters;

public abstract record QueryParams
{
    public virtual string SortColumn { get; init; } = "id";
    public virtual string SortOrder { get; init; } = "asc";
    public virtual int Page { get; init; } = 1;
    public virtual int PageSize { get; init; } = 10;
    public virtual string? SearchTerm { get; init; } = null;
}

public record UserQueryParams : QueryParams
{
    public override string SortColumn { get; init; } = "LastName";
}

public record ClientQueryParams : QueryParams
{
    public override string SortColumn { get; init; } = "LastName";
}