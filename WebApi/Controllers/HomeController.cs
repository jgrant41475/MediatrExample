using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Cars.Commands;
using Services.Cars.Queries;
using Services.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("Cars")]
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public Task<IEnumerable<Car>> Get()
        {
            return _mediator.Send(new GetAllCarsQuery());
        }

        [HttpPost]
        public Task<Response<Car>> Post([FromBody] CreateCarCommand command)
        {
            return _mediator.Send(command);
        }
    }
}