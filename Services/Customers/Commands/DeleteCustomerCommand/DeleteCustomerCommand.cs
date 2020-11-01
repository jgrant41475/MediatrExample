using System;
using MediatR;

namespace Services.Customers.Commands.DeleteCustomerCommand
{
    public class DeleteCustomerCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}