using System.Collections.Generic;
using MediatR;
using Services.Models;

namespace Services.ProductOrders.Queries.ListProductOrders
{
    public class ListProductOrdersQuery : IRequest<IEnumerable<ProductOrder>>
    { }
}