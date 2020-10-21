using System.Collections.Generic;
using MediatR;
using Services.Models;

namespace Services.Products.Queries.ListAnimalsQuery
{
    public class ListAnimalsQuery : IRequest<IEnumerable<Product>>
    {
        
    }
}