using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;
using Services.Validators;

namespace Services.Customers.Commands.UpdateCustomerInfoCommand
{
    public class UpdateCustomerInfoCommandHandler : IRequestHandler<UpdateCustomerInfoCommand, Customer>
    {
        private readonly PetShopContext _context;

        public UpdateCustomerInfoCommandHandler(PetShopContext context)
        {
            _context = context;
        }
        
        public async Task<Customer> Handle(UpdateCustomerInfoCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FindAsync(request.Id);
            if (customer is null)
            {
                return await Task.FromResult<Customer>(null);
            }

            customer.FirstName = request.FirstName ?? customer.FirstName;
            customer.LastName = request.LastName ?? customer.LastName;
            customer.Address = request.Address ?? customer.Address;
            customer.Email = request.Email ?? customer.Email;
            customer.Phone = request.Phone ?? customer.Phone;
            
            var customerValidator = new CustomerValidator();
            var validationResult = await customerValidator.ValidateAsync(customer, cancellationToken);

            if (!validationResult.IsValid)
            {
                return await Task.FromResult<Customer>(null);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await Task.FromResult<Customer>(customer);
        }
    }
}