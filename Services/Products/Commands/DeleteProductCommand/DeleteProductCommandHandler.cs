using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Products.Commands.DeleteProductCommand
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly PetShopContext _context;

        public DeleteProductCommandHandler(PetShopContext context)
        {
            _context = context;
        }
        
        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(
                r => request.Id.Equals(r.Id) && r.DeletedDateUtc == null, cancellationToken);
            if (product is null)
            {
                return await Task.FromResult(false);
            }

            product.DeletedDateUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync(CancellationToken.None);

            return await Task.FromResult(true);
        }
    }
}