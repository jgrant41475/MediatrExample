﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Services.Data;
using Services.Models;

namespace Services.ProductOrders.Queries.ListProductOrders
{
    public class ListProductOrdersQueryHandler : IRequestHandler<ListProductOrdersQuery, IEnumerable<ProductOrder>>
    {
        private readonly PetShopContext _context;

        public ListProductOrdersQueryHandler(PetShopContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<ProductOrder>> Handle(ListProductOrdersQuery request, CancellationToken cancellationToken)
        {
            var allProductOrders = _context.ProductOrders;

            return await Task.FromResult(allProductOrders);
        }
    }
}