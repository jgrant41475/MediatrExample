using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Services.Data;
using Services.Products.Commands.CreateProductCommand;
using Services.Products.Commands.DeleteProductCommand;
using Services.Products.Commands.UpdateProductInfoCommand;
using Services.Products.Queries.GetProductByIdQuery;
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
            var createProducts =
                _fixture.Build<CreateProductCommand>().CreateMany(numberOfProductsToCreate);

            var createProductCommandHandler = new CreateProductCommandHandler(_petShopContext);
            foreach (var product in createProducts)
            {
                await createProductCommandHandler.Handle(product, CancellationToken.None);
            }

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
            var products = await new ListAllProductsQueryHandler(_petShopContext)
                .Handle(new ListAllProductsQuery(), CancellationToken.None);
            
            Assert.Empty(products);
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
            var products = await new ListAllProductsQueryHandler(_petShopContext)
                .Handle(new ListAllProductsQuery(), CancellationToken.None);

            Assert.Empty(products);
            Assert.Equal(Guid.Empty, newProductId);
        }

        [Fact]
        public async void UpdateProductInfoCommand_UpdatesProduct()
        {
            // Arrange
            var createProduct = _fixture.Build<CreateProductCommand>().Create();
            var productId =
                await new CreateProductCommandHandler(_petShopContext).Handle(createProduct, CancellationToken.None);

            // Act
            var updatedProduct = _fixture.Build<UpdateProductInfoCommand>()
                .With(p => p.Id, productId)
                .Without(p => p.Name)
                .Create();

            var mockHandler = new UpdateProductInfoCommandHandler(_petShopContext);
            var result = await mockHandler.Handle(updatedProduct, CancellationToken.None);

            // Assert
            var product = await new GetProductByIdQueryHandler(_petShopContext)
                .Handle(new GetProductByIdQuery(productId), CancellationToken.None);

            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.IsAnimal, result.IsAnimal);
        }

        [Fact]
        public async void DeleteProductCommand_DeletesProduct()
        {
            // Arrange
            var createProduct = _fixture.Build<CreateProductCommand>().Create();
            var productId =
                await new CreateProductCommandHandler(_petShopContext).Handle(createProduct, CancellationToken.None);

            // Act
            var mockHandler = new DeleteProductCommandHandler(_petShopContext);
            var result = await mockHandler.Handle(
                new DeleteProductCommand{Id = productId}, CancellationToken.None);

            // Assert
            var productsCount = _petShopContext.Products.Count(c => c.DeletedDateUtc == null);
            var deletedProduct = await _petShopContext.Products.FindAsync(productId);

            Assert.True(result);
            Assert.Equal(0, productsCount);
            Assert.NotNull(deletedProduct.DeletedDateUtc);
        }
    }
}