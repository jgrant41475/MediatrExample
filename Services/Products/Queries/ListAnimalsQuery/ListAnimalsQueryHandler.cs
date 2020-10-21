using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;

namespace Services.Products.Queries.ListAnimalsQuery
{
    public class ListAnimalsQueryHandler : IRequestHandler<ListAnimalsQuery, IEnumerable<Product>>
    {
        private readonly PetShopContext _context;

        public ListAnimalsQueryHandler(PetShopContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<Product>> Handle(ListAnimalsQuery request, CancellationToken cancellationToken)
        {
            var allAnimals = _context.Products.Where(x => x.IsAnimal).ToList();

            return await Task.FromResult(allAnimals);
        }
    }
}