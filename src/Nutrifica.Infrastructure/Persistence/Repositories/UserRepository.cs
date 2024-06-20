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

    public Task<IPagedList<UserModel>> GetByFilterAsync(UserQueryParams requestQueryParams,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel?> GetDetailedByIdAsync(UserId id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}