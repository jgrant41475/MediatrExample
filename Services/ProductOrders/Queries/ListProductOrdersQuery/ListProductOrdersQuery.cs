using System.Collections.Generic;
using MediatR;
using Services.Models;

namespace Services.ProductOrders.Queries.ListProductOrdersQuery
{
    public class ListProductOrdersQuery : IRequest<IEnumerable<ProductOrder>>
    { }
}