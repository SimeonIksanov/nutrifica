using Nutrifica.Api.Contracts.Users.Responses;
using Nutrifica.Application.Abstractions.Messaging;
using Nutrifica.Application.Shared;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Users.Get;

public record GetUsersQuery(QueryParams QueryParams) : IQuery<PagedList<UserDto>>;