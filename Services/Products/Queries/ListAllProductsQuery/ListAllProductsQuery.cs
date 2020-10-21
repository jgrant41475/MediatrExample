using System.Collections.Generic;
using MediatR;
using Services.Models;

namespace Services.Products.Queries.ListAllProductsQuery
{
    public class ListAllProductsQuery : IRequest<IEnumerable<Product>>
    {
        
    }
}