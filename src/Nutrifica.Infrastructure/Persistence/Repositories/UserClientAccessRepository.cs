using Nutrifica.Application.Interfaces.Services.Persistence;
using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Infrastructure.Persistence.Repositories;

public class UserClientAccessRepository : IUserClientAccessRepository
{
    private readonly AppDbContext _context;

    public UserClientAccessRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(UserClientAccess userClientAccess) => _context.UserClientAccesses.Add(userClientAccess);

    public void Delete(UserClientAccess userClientAccess) => _context.UserClientAccesses.Remove(userClientAccess);
}