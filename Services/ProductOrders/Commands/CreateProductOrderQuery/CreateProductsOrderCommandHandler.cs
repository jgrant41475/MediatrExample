using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;
using Services.Validators;

namespace Services.ProductOrders.Commands.CreateProductOrderQuery
{
    public class CreateProductsOrderCommandHandler : IRequestHandler<CreateProductOrderCommand, Guid>
    {
        private readonly PetShopContext _context;

        public CreateProductsOrderCommandHandler(PetShopContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateProductOrderCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId);
            var product = await _context.Products.FindAsync(request.ProductId);

            if (customer is null || product is null)
            {
                return await Task.FromResult(Guid.Empty);
            }

            var order = new Order
            {
                CustomerId = customer.Id,
                OrderPlaced = DateTime.UtcNow,
            };

            await _context.Orders.AddAsync(order, cancellationToken);

            var orderValidator = new OrderValidator();
            var orderValidationResult = await orderValidator.ValidateAsync(order, cancellationToken);

            if (!orderValidationResult.IsValid)
            {
                return await Task.FromResult(Guid.Empty);
            }

            var productOrder = new ProductOrder
            {
                Quantity = request.Quantity,
                OrderId = order.Id,
                ProductId = product.Id,
            };

            await _context.ProductOrders.AddAsync(productOrder, cancellationToken);

            var productOrderValidator = new ProductOrderValidator();
            var productOrderValidationResult = 
                await productOrderValidator.ValidateAsync(productOrder, cancellationToken);

            if (!productOrderValidationResult.IsValid)
            {
                return await Task.FromResult(Guid.Empty);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(productOrder.Id);
        }
    }
}