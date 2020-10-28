﻿using FluentValidation;
using Services.Models;

namespace Services.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotNull().NotEmpty();
            RuleFor(p => p.Description).NotNull().NotEmpty();
            RuleFor(p => p.Price).GreaterThanOrEqualTo(0);
        }
    }
}