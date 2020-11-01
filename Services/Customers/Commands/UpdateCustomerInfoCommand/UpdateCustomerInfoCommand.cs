using System;
using MediatR;
using Services.Models;

namespace Services.Customers.Commands.UpdateCustomerInfoCommand
{
    public class UpdateCustomerInfoCommand : IRequest<Customer>
    {
        public Guid Id { get; set; }
        
        #nullable enable

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        
        #nullable disable
    }
}