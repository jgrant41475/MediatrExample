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

        public ProductTests()
        {
            _fixture = new Fixture();
        }

        private static PetShopContext GenerateMockDbContext()
        {
            var options = new DbContextOptionsBuilder<PetShopContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            
            return new PetShopContext(options);
        }

        [Fact]
        public async void ListProductsQuery_ReturnsAllProducts()
        {
            const int numberOfProductsToCreate = 5;

            // Arrange
            await using var mockPetShopContext = GenerateMockDbContext();

            var listOfProducts = _fixture.Build<Product>().CreateMany(numberOfProductsToCreate);

            await mockPetShopContext.Products.AddRangeAsync(listOfProducts);
            await mockPetShopContext.SaveChangesAsync();

            // Act
            var mockHandler = new ListAllProductsQueryHandler(mockPetShopContext);

            var mockHandlerResult = 
                await mockHandler.Handle(new ListAllProductsQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(numberOfProductsToCreate, mockHandlerResult.Count());
        }

        [Fact]
        public async void CreateProductCommand_CreatesProduct()
        {
            // Arrange
            await using var mockPetShopContext = GenerateMockDbContext();

            var mockProduct = _fixture.Build<CreateProductCommand>().Create();

            // Act
            var createProductCommand = new CreateProductCommandHandler(mockPetShopContext);
            var newProductId = await createProductCommand.Handle(mockProduct, CancellationToken.None);

            // Assert
            var numberOfProducts = mockPetShopContext.Products.Count();

            Assert.Equal(1, numberOfProducts);
            Assert.NotEqual(Guid.Empty, newProductId);
        }

        [Fact]
        public async void CreateProductCommand_EmptyName_DoesNotCreateProduct()
        {
            // Arrange
            await using var mockPetShopContext = GenerateMockDbContext();

            var mockProduct = _fixture.Build<CreateProductCommand>()
                .With(p => p.Name, string.Empty)
                .Create();

            // Act
            var mockHandler = new CreateProductCommandHandler(mockPetShopContext);

            var newProductId = await mockHandler.Handle(mockProduct, CancellationToken.None);

            // Assert
            Assert.Equal(0, mockPetShopContext.Products.Count());
            Assert.Equal(Guid.Empty, newProductId);
        }

        [Fact]
        public async void CreateProductCommand_NegativePrice_DoesNotCreateProduct()
        {
            // Arrange
            await using var mockPetShopContext = GenerateMockDbContext();

            var mockProduct = _fixture.Build<CreateProductCommand>()
                .With(p => p.Price, -10.00M)
                .Create();

            // Act
            var mockHandler = new CreateProductCommandHandler(mockPetShopContext);
            var newProductId = await mockHandler.Handle(mockProduct, CancellationToken.None);

            // Assert
            Assert.Equal(0, mockPetShopContext.Products.Count());
            Assert.Equal(Guid.Empty, newProductId);
        }
    }
}