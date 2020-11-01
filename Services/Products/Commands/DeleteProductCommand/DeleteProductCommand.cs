using System;
using MediatR;

namespace Services.Products.Commands.DeleteProductCommand
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}