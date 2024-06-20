namespace Nutrifica.Application.Interfaces.Services;

public interface IPagedList<T>
{
    ICollection<T> Items { get; }
    int Page { get;}
    int PageSize { get; }
    int TotalCount { get; }
    bool HasNext { get; }
    bool HasPrev { get; }
    IPagedList<TResult> ProjectItems<TResult>(Func<T, TResult> func);
}