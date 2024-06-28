namespace Nutrifica.Shared.Wrappers;

public class PagedList<T>
{
    public PagedList() { }

    private PagedList(ICollection<T> items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public ICollection<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool HasNext => Page * PageSize < TotalCount;
    public bool HasPrev => Page > 1;

    public static PagedList<T> Create(ICollection<T> items, int page, int pageSize, int totalCount)
    {
        return new PagedList<T>(items, page, pageSize, totalCount);
    }
    // public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
    // {
    //     var totalCount = await query.CountAsync();
    //     var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    //     return new PagedList<T>(items, page, pageSize, totalCount);
    // }

    // public static async Task<PagedList<T>> CreateAsync(ISieveProcessor sieveProcessor, QueryParams queryParams,
    //     IQueryable<T> queryable, CancellationToken ct)
    // {
    //     SieveModel sieveModel = CreateSieveModel(queryParams);
    //     var filtered = sieveProcessor.Apply((SieveModel)sieveModel, queryable, applyPagination: false);
    //     int totalCount = await filtered.CountAsync(ct);
    //     var items = await sieveProcessor
    //         .Apply((SieveModel)sieveModel, filtered, applyFiltering: false, applySorting: false)
    //         .ToListAsync(ct);
    //     return new PagedList<T>(items, sieveModel.Page ?? 1, sieveModel.PageSize ?? items.Count, totalCount);
    // }

    // public IPagedList<TResult> ProjectItems<TResult>(Func<T, TResult> func)
    // {
    //     var projectedItems = Items
    //         .Select(func)
    //         .ToArray();
    //     return new PagedList<TResult>(projectedItems, Page, PageSize, TotalCount);
    // }
    //
    // private static SieveModel CreateSieveModel(QueryParams queryParams)
    // {
    //     SieveModel sieveModel = new SieveModel()
    //     {
    //         Filters = queryParams.Filters,
    //         Sorts = queryParams.Sorts,
    //         Page = queryParams.Page,
    //         PageSize = queryParams.PageSize
    //     };
    //     return sieveModel;
    // }
}