using MediatR;

using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{

}
