using System;
using FluentValidation;
using Services.Models;

namespace Services.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(o => o.Id).NotNull().NotEmpty();
            RuleFor(o => o.CustomerId).NotNull().NotEmpty();
            RuleFor(o => o.Customer).SetValidator(new CustomerValidator());
            RuleFor(o => o.OrderPlaced).NotNull().GreaterThanOrEqualTo(DateTime.MinValue);
        }
    }
}