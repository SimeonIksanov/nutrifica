using Microsoft.EntityFrameworkCore;

using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Application.Models.Users;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Infrastructure.Services.SortAndFilter;
using Nutrifica.Shared.Enums;
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
        var query = _context.Set<User>()
            .AsNoTracking()
            .Include(x => x.Account)
            .Select(user => new UserModel(
                user.Id.Value,
                user.Account.Username,
                user.FirstName.Value,
                user.MiddleName.Value,
                user.LastName.Value,
                user.Email.Value,
                user.PhoneNumber.Value,
                user.Enabled,
                user.DisableReason,
                user.SupervisorId.Value,
                user.Role,
                user.CreatedOn));

        var pagedList =
            await query.SieveToPagedListAsync(_sieveProcessor, queryParams.ToSieveModel(), cancellationToken);
        return pagedList;
    }

    public async Task<ICollection<UserShortModel>> GetManagers(UserId supervisorId, CancellationToken cancellationToken)
    {
        // TODO Выборка пользователей, которые подчиняются supervisorId
        var query = from user in _context.Users.AsNoTracking()
            where user.Role != UserRole.Operator
            select new UserShortModel(
                user.Id.Value,
                user.FirstName.Value,
                user.MiddleName.Value,
                user.LastName.Value);
        return await query.ToListAsync(cancellationToken);
    }

    public Task<UserModel?> GetDetailedByIdAsync(UserId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}