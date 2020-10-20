using System.Threading;
using System.Threading.Tasks;
using Services.Models;
using Services.Wrappers;

namespace Services.Cars.Commands
{
    public class CreateCarCommand : IRequestWrapper<Car>
    {
        
    }

    public class CreateCarCommandHandler : IHandlerWrapper<CreateCarCommand, Car>
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