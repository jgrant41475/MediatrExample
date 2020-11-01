using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;

namespace Services.Customers.Queries.ListCustomersQuery
{
    public class ListCustomersQueryHandler : IRequestHandler<ListCustomersQuery, IEnumerable<Customer>>
    {
        private readonly PetShopContext _context;

        public ListCustomersQueryHandler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> Handle(ListCustomersQuery request, CancellationToken cancellationToken)
        {
            var allCustomers = _context.Customers.Where(x => x.DeletedDateUtc == null);

            return await Task.FromResult(allCustomers);
        }
    }
}