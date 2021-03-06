﻿using FluentValidation;
using Services.Models;

namespace Services.Validators
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();
            RuleFor(c => c.FirstName).NotNull().NotEmpty();
            RuleFor(c => c.LastName).NotNull().NotEmpty();
            RuleFor(c => c.Address).NotNull().NotEmpty();
            RuleFor(c => c.CreateDateUtc).NotNull().NotEmpty();
        }
    }
}