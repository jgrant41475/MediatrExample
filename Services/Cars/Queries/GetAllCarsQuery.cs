using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Models;

namespace Services.Cars.Queries
{
    public class GetAllCarsQuery : BaseRequest, IRequest<IEnumerable<Car>> { }

    public class GetAllCarsQueryHandler : IRequestHandler<GetAllCarsQuery, IEnumerable<Car>>
    {
        public GetAllCarsQueryHandler()
        {
            
        }
        
        public async Task<IEnumerable<Car>> Handle(GetAllCarsQuery request, CancellationToken cancellationToken)
        {
            return new[]
            {
                new Car {Name = $"Ford {request.UserId}"},
                new Car {Name = "Toyota"}, 
            };
        }
    }
}