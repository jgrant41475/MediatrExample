using System;
using MediatR;

namespace Services.ProductOrders.Commands.DeleteProductOrderCommand
{
    public class DeleteProductOrderCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}