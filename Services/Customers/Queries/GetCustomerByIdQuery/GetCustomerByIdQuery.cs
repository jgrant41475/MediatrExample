using System;
using MediatR;
using Services.Models;

namespace Services.Customers.Queries.GetCustomerByIdQuery
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public Guid Id { get; set; }
    }
}