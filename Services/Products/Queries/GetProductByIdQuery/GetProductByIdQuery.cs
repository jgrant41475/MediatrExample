using System;
using MediatR;
using Services.Models;

namespace Services.Products.Queries.GetProductByIdQuery
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public Guid Id { get; }

        public GetProductByIdQuery(Guid productId)
        {
            Id = productId;
        }
    }
}