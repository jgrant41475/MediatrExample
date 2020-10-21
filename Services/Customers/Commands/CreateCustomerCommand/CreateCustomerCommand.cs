using System;
using MediatR;

namespace Services.Customers.Commands.CreateCustomerCommand
{
    public class CreateCustomerCommand : IRequest<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }
    }
    
    
}