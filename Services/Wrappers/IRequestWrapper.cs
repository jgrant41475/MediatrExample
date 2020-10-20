using MediatR;
using Services.Cars.Commands;
using Services.Models;

namespace Services.Wrappers
{
    public interface IRequestWrapper<T> : IRequest<Response<T>>
    {}

    public interface IHandlerWrapper<TIn, TOut> :
        IRequestHandler<CreateCarCommand, Response<Car>>
        where TIn : IRequestWrapper<TOut>
    {}
}