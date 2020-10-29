using System.Data;
using FluentValidation;
using Services.Models;

namespace Services.Validators
{
    public class ProductOrderValidator : AbstractValidator<ProductOrder>
    {
        public ProductOrderValidator()
        {
            RuleFor(p => p.Id).NotNull().NotEmpty();
            RuleFor(p => p.Order).SetValidator(new OrderValidator());
            RuleFor(p => p.OrderId).NotNull().NotEmpty();
            RuleFor(p => p.Product).SetValidator(new ProductValidator());
            RuleFor(p => p.ProductId).NotNull().NotEmpty();
            RuleFor(p => p.Quantity).GreaterThanOrEqualTo(1);
        }
    }
}