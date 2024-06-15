using MediatR;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{

}
