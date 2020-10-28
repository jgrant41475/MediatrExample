using System.Collections.Generic;
using MediatR;
using Services.Models;

namespace Services.Customers.Queries.ListCustomersQuery
{
    public class ListCustomersQuery : IRequest<IEnumerable<Customer>>
    {
        
    }
}