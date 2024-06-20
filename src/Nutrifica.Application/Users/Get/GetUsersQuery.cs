using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Interfaces.Services;
using Nutrifica.Shared.QueryParameters;

namespace Nutrifica.Application.Users.Get;

public record GetUsersQuery(
    UserQueryParams QueryParams) : IQuery<IPagedList<UserResponse>>;