using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Customers.Commands.DeleteCustomerCommand
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly PetShopContext _context;

        public DeleteCustomerCommandHandler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer =
                await _context.Customers.FirstOrDefaultAsync(
                    x => request.Id.Equals(x.Id) && x.DeletedDateUtc == null, cancellationToken);
            if (customer is null)
            {
                return await Task.FromResult(false);
            }

            customer.DeletedDateUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(true);
        }
    }
}