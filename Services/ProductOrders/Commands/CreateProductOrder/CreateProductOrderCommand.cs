using System;
using MediatR;

namespace Services.ProductOrders.Commands.CreateProductOrder
{
    public class CreateProductOrderCommand : IRequest<Guid>
    {
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }

        public Guid CustomerId { get; set; }
    }
}