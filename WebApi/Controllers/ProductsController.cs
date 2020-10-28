using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Models;
using Services.Products.Commands.CreateProductCommand;
using Services.Products.Queries.GetProductByIdQuery;
using Services.Products.Queries.ListAllProductsQuery;
using Services.Products.Queries.ListAnimalsQuery;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Route("product")]
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("")]
        [Route("list")]
        public async Task<IEnumerable<Product>> ListProducts() =>
            await _mediator.Send(new ListAllProductsQuery());

        [HttpGet]
        [Route("{productId}")]
        public async Task<Product> GetProductById([FromRoute] Guid productId) =>
            await _mediator.Send(new GetProductByIdQuery(productId));

        [HttpGet]
        [Route("list/animals")]
        public async Task<IEnumerable<Product>> ListAnimals() =>
            await _mediator.Send(new ListAnimalsQuery());

        [HttpPost]
        [Route("create")]
        public async Task<Guid> CreateProduct([FromBody] CreateProductCommand command) =>
            await _mediator.Send(command);
    }
}