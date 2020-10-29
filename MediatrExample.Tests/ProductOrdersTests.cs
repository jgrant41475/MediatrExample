using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Models;
using Services.ProductOrders.Commands.CreateProductOrder;
using Services.ProductOrders.Queries.GetProductOrderByIdQuery;
using Services.ProductOrders.Queries.ListProductOrders;
using Xunit;

namespace MediatrExample.Tests
{
    public class ProductOrdersTests
    {
        private readonly IFixture _fixture;
        private readonly PetShopContext _petShopContext;

        public ProductOrdersTests()
        {
            _fixture = new Fixture();
            
            var options = new DbContextOptionsBuilder<PetShopContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _petShopContext = new PetShopContext(options);
        }

        [Fact]
        public async void ListProductOrdersQuery_ListsAllProductOrders()
        {
            const int numberOfProductOrdersToCreate = 5;

            // Arrange
            var mockProductOrders = 
                _fixture.Build<ProductOrder>().CreateMany(numberOfProductOrdersToCreate);

            await _petShopContext.ProductOrders.AddRangeAsync(mockProductOrders, CancellationToken.None);
            await _petShopContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var mockHandler = new ListProductOrdersQueryHandler(_petShopContext);
            var handlerResult = 
                await mockHandler.Handle(new ListProductOrdersQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(numberOfProductOrdersToCreate, handlerResult.Count());
        }

        [Fact]
        public async void GetProductOrderByIdQuery_GetsProductOrder()
        {
            // Arrange
            var mockProductOrder = 
                _fixture.Build<ProductOrder>().Create();

            await _petShopContext.ProductOrders.AddAsync(mockProductOrder, CancellationToken.None);
            await _petShopContext.SaveChangesAsync(CancellationToken.None);

            // Act
            var mockHandler = new GetProductOrderByIdQueryHandler(_petShopContext);
            var handlerResult =
                await mockHandler.Handle(new GetProductOrderByIdQuery(mockProductOrder.Id), CancellationToken.None);

            // Assert
            Assert.Equal(mockProductOrder.Id, handlerResult.Id);
        }

        [Fact]
        public async void CreateProductOrderCommand_CreatesProductOrder()
        {
            // Arrange
            var mockCustomer = _fixture.Build<Customer>().Create();
            await _petShopContext.Customers.AddAsync(mockCustomer, CancellationToken.None);

            var mockProduct = _fixture.Build<Product>().Create();
            await _petShopContext.Products.AddAsync(mockProduct);
            
            var mockProductOrder = _fixture.Build<CreateProductOrderCommand>()
                .With(p => p.CustomerId, mockCustomer.Id)
                .With(p => p.ProductId, mockProduct.Id)
                .Create();

            // Act
            var mockHandler = new CreateProductsOrderCommandHandler(_petShopContext);
            var newProductOrderId = await mockHandler.Handle(mockProductOrder, CancellationToken.None);

            // Assert
            Assert.Equal(1, _petShopContext.ProductOrders.Count());
            Assert.NotEqual(Guid.Empty, newProductOrderId);
        }

        [Fact]
        public async void CreateProductOrderCommand_InvalidProductId_DoesNotCreateProductOrder()
        {
            // Arrange
            var mockCustomer = _fixture.Build<Customer>().Create();
            await _petShopContext.Customers.AddAsync(mockCustomer, CancellationToken.None);

            var mockProductOrder = _fixture.Build<CreateProductOrderCommand>()
                .With(p => p.CustomerId, mockCustomer.Id)
                .With(p => p.ProductId, Guid.NewGuid())
                .Create();

            // Act
            var mockHandler = new CreateProductsOrderCommandHandler(_petShopContext);
            var newProductOrderId = await mockHandler.Handle(mockProductOrder, CancellationToken.None);

            // Assert
            Assert.Equal(0, _petShopContext.ProductOrders.Count());
            Assert.Equal(Guid.Empty, newProductOrderId);
        }
        
        [Fact]
        public async void CreateProductOrderCommand_InvalidCustomerId_DoesNotCreateProductOrder()
        {
            // Arrange
            var mockProduct = _fixture.Build<Product>().Create();
            await _petShopContext.Products.AddAsync(mockProduct);

            var mockProductOrder = _fixture.Build<CreateProductOrderCommand>()
                .With(p => p.CustomerId, Guid.NewGuid())
                .With(p => p.ProductId, mockProduct.Id)
                .Create();

            // Act
            var mockHandler = new CreateProductsOrderCommandHandler(_petShopContext);
            var newProductOrderId = await mockHandler.Handle(mockProductOrder, CancellationToken.None);

            // Assert
            Assert.Equal(0, _petShopContext.ProductOrders.Count());
            Assert.Equal(Guid.Empty, newProductOrderId);
        }
        
        [Fact]
        public async void CreateProductOrderCommand_NegativeQuantity_DoesNotCreateProductOrder()
        {
            // Arrange
            var mockCustomer = _fixture.Build<Customer>().Create();
            await _petShopContext.Customers.AddAsync(mockCustomer, CancellationToken.None);

            var mockProduct = _fixture.Build<Product>().Create();
            await _petShopContext.Products.AddAsync(mockProduct);

            var mockProductOrder = _fixture.Build<CreateProductOrderCommand>()
                .With(p => p.CustomerId, mockCustomer.Id)
                .With(p => p.ProductId, mockProduct.Id)
                .With(p => p.Quantity, -1)
                .Create();

            // Act
            var mockHandler = new CreateProductsOrderCommandHandler(_petShopContext);
            var newProductOrderId = await mockHandler.Handle(mockProductOrder, CancellationToken.None);

            // Assert
            Assert.Equal(0, _petShopContext.ProductOrders.Count());
            Assert.Equal(Guid.Empty, newProductOrderId);
        }
    }
}