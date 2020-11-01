using System;
using MediatR;
using Services.Models;

namespace Services.Products.Commands.UpdateProductInfoCommand
{
    public class UpdateProductInfoCommand : IRequest<Product>
    {
        public Guid Id { get; set; }
        
        #nullable enable
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public bool? IsAnimal { get; set; }
        #nullable disable
    }
}