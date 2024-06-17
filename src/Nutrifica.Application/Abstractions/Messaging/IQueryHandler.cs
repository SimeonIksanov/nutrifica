using MediatR;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{

}
