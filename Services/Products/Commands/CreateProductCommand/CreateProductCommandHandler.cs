using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;
using Services.Validators;

namespace Services.Products.Commands.CreateProductCommand
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly PetShopContext _context;

        public CreateProductCommandHandler(PetShopContext context)
        {
            _context = context;
        }
        
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var newProduct = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                IsAnimal = request.IsAnimal,
                CreateDateUtc = DateTime.UtcNow,
                DeletedDateUtc = null,
            };

            var doesProductExist = 
                _context.Products.FirstOrDefault(x => x.Name.Equals(newProduct.Name)) != null;
            if (doesProductExist)
            {
                return await Task.FromResult(Guid.Empty);
            }

            await _context.Products.AddAsync(newProduct, cancellationToken);

            var productValidator = new ProductValidator();
            var productValidationResult = await productValidator.ValidateAsync(newProduct, cancellationToken);

            if (!productValidationResult.IsValid)
            {
                return await Task.FromResult(Guid.Empty);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(newProduct.Id);
        }
    }
}