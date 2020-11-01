using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;
using Services.Validators;

namespace Services.Products.Commands.UpdateProductInfoCommand
{
    public class UpdateProductInfoCommandHandler : IRequestHandler<UpdateProductInfoCommand, Product>
    {
        private readonly PetShopContext _context;

        public UpdateProductInfoCommandHandler(PetShopContext context)
        {
            _context = context;
        }
        
        public async Task<Product> Handle(UpdateProductInfoCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);
            if (product is null)
            {
                return await Task.FromResult<Product>(null);
            }

            product.Name = request.Name ?? product.Name;
            product.Description = request.Description ?? product.Description;
            product.Price = request.Price ?? product.Price;
            product.IsAnimal = request.IsAnimal ?? product.IsAnimal;

            var productValidator = new ProductValidator();
            var validationResult = await productValidator.ValidateAsync(product, cancellationToken);

            if (!validationResult.IsValid)
            {
                return await Task.FromResult<Product>(null);
            }
            
            await _context.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(product);
        }
    }
}