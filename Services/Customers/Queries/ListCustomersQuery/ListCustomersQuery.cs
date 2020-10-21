using System.Collections;
using System.Collections.Generic;
using MediatR;
using Services.Models;

namespace Services.Customers.Queries
{
    public class ListCustomersQuery : IRequest<IEnumerable<Customer>>
    {
        
    }
}