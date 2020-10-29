using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;
using Services.Validators;

namespace Services.Customers.Commands.CreateCustomerCommand
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
    {
        private readonly PetShopContext _context;

        public CreateCustomerCommandHandler(PetShopContext context)
        {
            _context = context;
        }
        
        public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var newCustomer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                Email = request.Email,
            };

            var doesCustomerExist = _context.Customers.FirstOrDefault(x =>
                x.FirstName.Equals(newCustomer.FirstName) &&
                x.LastName.Equals(newCustomer.LastName)) != null;

            if (doesCustomerExist)
            {
                return await Task.FromResult(Guid.Empty);
            }

            await _context.Customers.AddAsync(newCustomer, cancellationToken);

            var customerValidator = new CustomerValidator();
            var customerValidationResult = await customerValidator.ValidateAsync(newCustomer, cancellationToken);

            if (!customerValidationResult.IsValid)
            {
                return await Task.FromResult(Guid.Empty);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(newCustomer.Id);
        }
    }
}