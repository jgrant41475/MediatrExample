using System;
using MediatR;
using Services.Models;

namespace Services.ProductOrders.Queries.GetProductOrderByIdQuery
{
    public class GetProductOrderByIdQuery : IRequest<ProductOrder>
    {
        public Guid Id { get; set; }

        public GetProductOrderByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}