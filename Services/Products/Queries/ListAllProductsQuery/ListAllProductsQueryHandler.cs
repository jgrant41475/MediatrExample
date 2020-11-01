using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;

namespace Services.Products.Queries.ListAllProductsQuery
{
    public class ListAllProductsQueryHandler : IRequestHandler<ListAllProductsQuery, IEnumerable<Product>>
    {
        private readonly PetShopContext _petShopContext;

        public ListAllProductsQueryHandler(PetShopContext petShopContext)
        {
            _petShopContext = petShopContext;
        }

        public async Task<IEnumerable<Product>> Handle(ListAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = _petShopContext.Products.Where(x => x.DeletedDateUtc == null);

            return await Task.FromResult(products);
        }
    }
}