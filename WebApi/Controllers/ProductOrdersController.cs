﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Models;
using Services.ProductOrders.Commands.CreateProductOrderQuery;
using Services.ProductOrders.Commands.DeleteProductOrderCommand;
using Services.ProductOrders.Queries.GetProductOrderByIdQuery;
using Services.ProductOrders.Queries.ListProductOrdersQuery;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Route("ProductOrder")]
    public class ProductOrdersController : Controller
    {
        private readonly IMediator _mediator;

        public ProductOrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        [Route("list")]
        public async Task<IEnumerable<ProductOrder>> ListAllProductOrders() =>
            await _mediator.Send(new ListProductOrdersQuery());

        [HttpGet]
        [Route("{productOrderId}")]
        public async Task<ProductOrder> GetProductOrderById([FromRoute] Guid productOrderId) =>
            await _mediator.Send(new GetProductOrderByIdQuery(productOrderId));

        [HttpPost]
        [Route("create")]
        public async Task<Guid> CreateProductOrder(CreateProductOrderCommand request) =>
            await _mediator.Send(request);

        [HttpDelete]
        [Route("{productOrderId}/delete")]
        public async Task<bool> DeleteProductOrder([FromRoute] Guid productOrderId) =>
            await _mediator.Send(new DeleteProductOrderCommand {Id = productOrderId});
    }
}