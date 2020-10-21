﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;

namespace Services.Customers.Queries
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
            return _context.Customers.ToList();
        }
    }
}