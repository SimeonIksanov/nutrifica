using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Users;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Shared.QueryParameters;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(User user)
    {
        _context.Set<User>().Add(user);
    }

    public async Task<User?> GetByIdAsync(UserId userId, CancellationToken ct = default)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(x => x.Id == userId, ct);
    }

    public async Task<User?> GetByIdIncludeAccountAsync(UserId userId, CancellationToken ct = default)
    {
        return await _context.Set<User>()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Id == userId, ct);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _context.Set<User>()
            .Include(x => x.Account)
            .FirstOrDefaultAsync(x => x.Account.Username == username, cancellationToken: ct);
    }

    public async Task<IPagedList<UserModel>> GetByFilterAsync(UserQueryParams requestQueryParams,
        CancellationToken cancellationToken)
    {
        IQueryable<User> query = _context.Set<User>();

        query = FilterWithSearchTerm(query, requestQueryParams);

        query = requestQueryParams.SortOrder == "desc"
            ? query.OrderByDescending(GetSortProperty(requestQueryParams))
            : query.OrderBy(GetSortProperty(requestQueryParams));

        var users = query
            .GroupJoin(_context.Set<User>(),
                o => o.SupervisorId,
                i => i.Id,
                (i, ol) => new { Employee = i, Supervisors = ol })
            .SelectMany(arg => arg.Supervisors.DefaultIfEmpty(), (e, s) => new UserModel(
                e.Employee.Id.Value,
                e.Employee.Account.Username,
                e.Employee.FirstName.Value,
                e.Employee.MiddleName.Value,
                e.Employee.LastName.Value,
                e.Employee.Email.Value,
                e.Employee.PhoneNumber.Value,
                e.Employee.Enabled,
                e.Employee.DisableReason,
                Equals(null, s) ? null : s.Id.Value,
                Equals(null, s) ? "" : s.FullName,
                e.Employee.Role,
                e.Employee.CreatedAt));

        return await PagedList<UserModel>.CreateAsync(users, requestQueryParams.Page, requestQueryParams.PageSize);
    }

    public Task<UserModel?> GetDetailedByIdAsync(UserId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private IQueryable<User> FilterWithSearchTerm(IQueryable<User> queryable, UserQueryParams requestQueryParams) =>
        string.IsNullOrWhiteSpace(requestQueryParams.SearchTerm)
            ? queryable
            : queryable.Where(x =>
                ((string)x.FirstName).Contains(requestQueryParams.SearchTerm)
                || ((string)x.MiddleName).Contains(requestQueryParams.SearchTerm)
                || ((string)x.LastName).Contains(requestQueryParams.SearchTerm)
                || ((string)x.PhoneNumber).Contains(requestQueryParams.SearchTerm)
            );

    private static Expression<Func<User, object>> GetSortProperty(UserQueryParams requestQueryParams) =>
        requestQueryParams.SortColumn.ToLower() switch
        {
            "firstname" => u => u.FirstName,
            "middlename" => u => u.MiddleName,
            "lastname" => u => u.LastName,
            "role" => u => u.Role,
            "email" => u => u.Email,
            "phonenumber" => u => u.PhoneNumber,
            _ => user => user.CreatedAt
        };
}