using Ardalis.Result;
using MediatR;

namespace BookWorm.SharedKernel.Command;

public interface ICommand : ICommand<Result>;

public interface ICommand<out TResponse> : IRequest<TResponse>;
