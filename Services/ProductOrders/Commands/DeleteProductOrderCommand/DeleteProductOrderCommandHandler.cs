using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.ProductOrders.Commands.DeleteProductOrderCommand
{
    public class DeleteProductOrderCommandHandler : IRequestHandler<DeleteProductOrderCommand, bool>
    {
        private readonly PetShopContext _context;

        public DeleteProductOrderCommandHandler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteProductOrderCommand request, CancellationToken cancellationToken)
        {
            var productOrder = await _context.ProductOrders
                .Include(p => p.Order.Customer)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(p => request.Id.Equals(p.Id) && p.DeletedDateUtc == null, cancellationToken);

            if (productOrder is null)
            {
                return await Task.FromResult(false);
            }

            var utcNow = DateTime.UtcNow;
            
            productOrder.DeletedDateUtc = utcNow;
            productOrder.Order.DeletedDateUtc = utcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(true);
        }
    }
}