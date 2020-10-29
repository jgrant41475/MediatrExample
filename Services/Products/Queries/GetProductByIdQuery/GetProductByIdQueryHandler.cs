using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;

namespace Services.Products.Queries.GetProductByIdQuery
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private readonly PetShopContext _context;

        public GetProductByIdQueryHandler(PetShopContext context)
        {
            _context = context;
        }
        
        public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.Id);

            return product;
        }
    }
}