﻿using Nutrifica.Application.Models.Users;
using Nutrifica.Application.Shared;
using Nutrifica.Domain.Aggregates.UserAggregate;
using Nutrifica.Domain.Aggregates.UserAggregate.ValueObjects;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Interfaces.Services.Persistence;

public interface IUserRepository
{
    void Add(User user);
    Task<User?> GetByIdAsync(UserId userId, CancellationToken ct = default);
    Task<User?> GetByIdIncludeAccountAsync(UserId userId, CancellationToken ct = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<PagedList<UserModel>> GetByFilterAsync(QueryParams queryParams, CancellationToken cancellationToken);
    Task<UserModel?> GetDetailedByIdAsync(UserId id, CancellationToken cancellationToken);
}
