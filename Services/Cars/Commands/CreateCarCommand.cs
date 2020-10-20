using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Models;

namespace Services.Cars.Commands
{
    public class CreateCarCommand : IRequest<Response<Car>>
    {
        
    }

    public class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, Response<Car>>
    {
        public async Task<Response<Car>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
        {
            // return Response.Fail<Car>("Already Exists!");

            return Response.Ok(new Car {Name = "DFSDGFS"}, "Car Created!");

            /*return new Response<Car>
            {
                Data = new Car { Name = "New Car!" },
                Error = false,
                Message = "Success!"
            };*/
        }
    }
}