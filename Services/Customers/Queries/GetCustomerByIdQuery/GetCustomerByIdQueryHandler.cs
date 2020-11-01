using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Models;

namespace Services.Customers.Queries.GetCustomerByIdQuery
{
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly PetShopContext _context;

        public GetCustomerByIdQueryHandler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(
                c => request.Id.Equals(c.Id) && c.DeletedDateUtc == null, cancellationToken);

            return await Task.FromResult(customer);
        }
    }
}