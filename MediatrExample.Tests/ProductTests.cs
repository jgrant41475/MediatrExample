using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Models;
using Services.Products.Commands.CreateProductCommand;
using Services.Products.Queries.ListAllProductsQuery;
using Xunit;

namespace MediatrExample.Tests
{
    public class ProductTests
    {
        private readonly IFixture _fixture;
        private readonly PetShopContext _petShopContext;

        public ProductTests()
        {
            _fixture = new Fixture();
            
            var options = new DbContextOptionsBuilder<PetShopContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _petShopContext = new PetShopContext(options);
        }

        [Fact]
        public async void ListProductsQuery_ReturnsAllProducts()
        {
            const int numberOfProductsToCreate = 5;

            // Arrange
            var listOfProducts = _fixture.Build<Product>().CreateMany(numberOfProductsToCreate);

            await _petShopContext.Products.AddRangeAsync(listOfProducts);
            await _petShopContext.SaveChangesAsync();

            // Act
            var mockHandler = new ListAllProductsQueryHandler(_petShopContext);

            var mockHandlerResult = 
                await mockHandler.Handle(new ListAllProductsQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(numberOfProductsToCreate, mockHandlerResult.Count());
        }

        [Fact]
        public async void CreateProductCommand_CreatesProduct()
        {
            // Arrange
            var mockProduct = _fixture.Build<CreateProductCommand>().Create();

            // Act
            var createProductCommand = new CreateProductCommandHandler(_petShopContext);
            var newProductId = await createProductCommand.Handle(mockProduct, CancellationToken.None);

            // Assert
            var numberOfProducts = _petShopContext.Products.Count();

            Assert.Equal(1, numberOfProducts);
            Assert.NotEqual(Guid.Empty, newProductId);
        }

        [Fact]
        public async void CreateProductCommand_EmptyName_DoesNotCreateProduct()
        {
            // Arrange
            var mockProduct = _fixture.Build<CreateProductCommand>()
                .With(p => p.Name, string.Empty)
                .Create();

            // Act
            var mockHandler = new CreateProductCommandHandler(_petShopContext);

            var newProductId = await mockHandler.Handle(mockProduct, CancellationToken.None);

            // Assert
            Assert.Equal(0, _petShopContext.Products.Count());
            Assert.Equal(Guid.Empty, newProductId);
        }

        [Fact]
        public async void CreateProductCommand_NegativePrice_DoesNotCreateProduct()
        {
            // Arrange
            var mockProduct = _fixture.Build<CreateProductCommand>()
                .With(p => p.Price, -10.00M)
                .Create();

            // Act
            var mockHandler = new CreateProductCommandHandler(_petShopContext);
            var newProductId = await mockHandler.Handle(mockProduct, CancellationToken.None);

            // Assert
            Assert.Equal(0, _petShopContext.Products.Count());
            Assert.Equal(Guid.Empty, newProductId);
        }
    }
}