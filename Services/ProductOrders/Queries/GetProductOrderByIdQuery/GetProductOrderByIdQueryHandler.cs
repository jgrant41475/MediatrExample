using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Models;

namespace Services.ProductOrders.Queries.GetProductOrderByIdQuery
{
    public class GetProductOrderByIdQueryHandler : IRequestHandler<GetProductOrderByIdQuery, ProductOrder>
    {
        private readonly PetShopContext _context;

        public GetProductOrderByIdQueryHandler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<ProductOrder> Handle(GetProductOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var productOrder =
                await _context.ProductOrders
                    .Include(p => p.Product)
                    .Include(p => p.Order.Customer)
                    .FirstOrDefaultAsync(p => request.Id.Equals(p.Id), cancellationToken: cancellationToken);

            return productOrder;
        }
    }
}