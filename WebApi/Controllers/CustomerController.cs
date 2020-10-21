using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Customers.Commands.CreateCustomerCommand;
using Services.Customers.Queries;
using Services.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("")]
        [Route("list")]
        [HttpGet]
        public async Task<IEnumerable<Customer>> ListCustomers()
        {
            return await _mediator.Send(new ListCustomersQuery());
        }

        [Route("create")]
        [HttpPost]
        public async Task<Guid> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            return await _mediator.Send(command);
        }
    }
}