using System;
using MediatR;

namespace Services.Products.Commands.CreateProductCommand
{
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public bool IsAnimal { get; set; }
    }
}