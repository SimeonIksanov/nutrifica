using Nutrifica.Domain.Common.Models;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface IUserClientAccessRepository
{
    void Add(UserClientAccess userClientAccess);
    void Delete(UserClientAccess userClientAccess);
}