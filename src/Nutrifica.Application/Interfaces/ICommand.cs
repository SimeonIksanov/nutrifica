using MediatR;
using Nutrifica.Shared.Wrappers;

namespace Nutrifica.Application.Interfaces;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
