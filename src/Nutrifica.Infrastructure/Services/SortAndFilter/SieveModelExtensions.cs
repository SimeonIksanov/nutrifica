using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Shared;
using Nutrifica.Shared.Wrappers;

using Sieve.Models;
using Sieve.Services;

namespace Nutrifica.Infrastructure.Services.SortAndFilter;

public static class SieveModelExtensions
{
    public static SieveModel ToSieveModel(this QueryParams queryParams) =>
        new SieveModel
        {
            Page = queryParams.Page,
            PageSize = queryParams.PageSize,
            Filters = queryParams.Filters,
            Sorts = queryParams.Sorts
        };

    public static async Task<PagedList<T>> SieveToPagedListAsync<T>(this IQueryable<T> queryable,
        ISieveProcessor processor, SieveModel model, CancellationToken ct)
    {
        var filtered = processor.Apply(model, queryable, applyPagination: false);
        int totalCount = await filtered.CountAsync(ct);
        var items = await processor
            .Apply(model, filtered, applyFiltering: false, applySorting: false)
            .ToListAsync(ct);
        return PagedList<T>.Create(items, model.Page ?? 1, model.PageSize ?? items.Count, totalCount);
    }
}