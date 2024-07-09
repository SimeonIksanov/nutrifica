using Microsoft.EntityFrameworkCore;

using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Users;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Infrastructure.Services.SortAndFilter;
using Nutrifica.Shared.Wrappers;

using Sieve.Services;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ISieveProcessor _sieveProcessor;

    public UserRepository(AppDbContext context, ISieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
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

    public async Task<int> GetCountByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _context.Set<User>().Where(x => x.Account.Username == username).CountAsync(ct);
    }

    public async Task<PagedList<UserModel>> GetByFilterAsync(QueryParams queryParams,
        CancellationToken cancellationToken)
    {
        IQueryable<User> query = _context.Set<User>();

        var users = query
            .Include(x => x.Account)
            .GroupJoin(_context.Set<User>(),
                o => o.SupervisorId,
                i => i.Id,
                (i, ol) => new { Employee = i, Supervisors = ol })
            .SelectMany(arg => arg.Supervisors.DefaultIfEmpty(), (e, s) =>
                new UserModel(
                    e.Employee.Id.Value,
                    e.Employee.Account.Username,
                    e.Employee.FirstName.Value,
                    e.Employee.MiddleName.Value,
                    e.Employee.LastName.Value,
                    e.Employee.Email.Value,
                    e.Employee.PhoneNumber.Value,
                    e.Employee.Enabled,
                    e.Employee.DisableReason,
                    e.Employee.SupervisorId.Value,
                    e.Employee.Role,
                    e.Employee.CreatedAt));

        var pagedList = await users.SieveToPagedListAsync(_sieveProcessor, queryParams.ToSieveModel(), cancellationToken);
        return pagedList;
    }

    public Task<UserModel?> GetDetailedByIdAsync(UserId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}