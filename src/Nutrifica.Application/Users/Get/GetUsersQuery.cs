using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Application.Shared;

namespace Nutrifica.Application.Users.Get;

public record GetUsersQuery(
    QueryParams queryParams) : IQuery<IPagedList<UserResponse>>;