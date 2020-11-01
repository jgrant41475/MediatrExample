using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Services.Customers.Commands.CreateCustomerCommand;
using Services.Data;
using Services.ProductOrders.Commands.CreateProductOrderQuery;
using Services.ProductOrders.Commands.DeleteProductOrderCommand;
using Services.ProductOrders.Queries.GetProductOrderByIdQuery;
using Services.ProductOrders.Queries.ListProductOrdersQuery;
using Services.Products.Commands.CreateProductCommand;
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
            var createCustomer = _fixture.Build<CreateCustomerCommand>().Create();
            var customerId =
                await new CreateCustomerCommandHandler(_petShopContext).Handle(createCustomer, CancellationToken.None);

            var createProduct = _fixture.Build<CreateProductCommand>().Create();
            var productId =
                await new CreateProductCommandHandler(_petShopContext).Handle(createProduct, CancellationToken.None);

            var mockProductOrders = _fixture.Build<CreateProductOrderCommand>()
                .With(p => p.CustomerId, customerId)
                .With(p => p.ProductId, productId)
                .CreateMany(numberOfProductOrdersToCreate);

            var createProductOrderCommandHandler = new CreateProductsOrderCommandHandler(_petShopContext);
            foreach (var productOrder in mockProductOrders)
            {
                await createProductOrderCommandHandler.Handle(productOrder, CancellationToken.None);
            }

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
            var createCustomer = _fixture.Build<CreateCustomerCommand>().Create();
            var customerId =
                await new CreateCustomerCommandHandler(_petShopContext).Handle(createCustomer, CancellationToken.None);

            var createProduct = _fixture.Build<CreateProductCommand>().Create();
            var productId =
                await new CreateProductCommandHandler(_petShopContext).Handle(createProduct, CancellationToken.None);

            var mockProductOrder = _fixture.Build<CreateProductOrderCommand>()
                .With(p => p.CustomerId, customerId)
                .With(p => p.ProductId, productId)
                .Create();
            var productOrderId = await new CreateProductsOrderCommandHandler(_petShopContext)
                .Handle(mockProductOrder, CancellationToken.None);

            // Act
            var mockHandler = new GetProductOrderByIdQueryHandler(_petShopContext);
            var handlerResult =
                await mockHandler.Handle(new GetProductOrderByIdQuery(productOrderId), CancellationToken.None);

            // Assert
            Assert.Equal(productOrderId, handlerResult.Id);
        }

        [Fact]
        public async void CreateProductOrderCommand_CreatesProductOrder()
        {
            // Arrange
            var createCustomer = _fixture.Build<CreateCustomerCommand>().Create();
            var customerId =
                await new CreateCustomerCommandHandler(_petShopContext).Handle(createCustomer, CancellationToken.None);

            var createProduct = _fixture.Build<CreateProductCommand>().Create();
            var productId =
                await new CreateProductCommandHandler(_petShopContext).Handle(createProduct, CancellationToken.None);

            var mockProductOrder = _fixture.Build<CreateProductOrderCommand>()
                .With(p => p.CustomerId, customerId)
                .With(p => p.ProductId, productId)
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
            var createCustomer = _fixture.Build<CreateCustomerCommand>().Create();
            var customerId =
                await new CreateCustomerCommandHandler(_petShopContext).Handle(createCustomer, CancellationToken.None);

            var mockProductOrder = _fixture.Build<CreateProductOrderCommand>()
                .With(p => p.CustomerId, customerId)
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
            var createProduct = _fixture.Build<CreateProductCommand>().Create();
            var productId =
                await new CreateProductCommandHandler(_petShopContext).Handle(createProduct, CancellationToken.None);


            var mockProductOrder = _fixture.Build<CreateProductOrderCommand>()
                .With(p => p.CustomerId, Guid.NewGuid())
                .With(p => p.ProductId, productId)
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
            var createCustomer = _fixture.Build<CreateCustomerCommand>().Create();
            var customerId =
                await new CreateCustomerCommandHandler(_petShopContext).Handle(createCustomer, CancellationToken.None);

            var createProduct = _fixture.Build<CreateProductCommand>().Create();
            var productId =
                await new CreateProductCommandHandler(_petShopContext).Handle(createProduct, CancellationToken.None);

            var mockProductOrder = _fixture.Build<CreateProductOrderCommand>()
                .With(p => p.CustomerId, customerId)
                .With(p => p.ProductId, productId)
                .With(p => p.Quantity, -1)
                .Create();

            // Act
            var mockHandler = new CreateProductsOrderCommandHandler(_petShopContext);
            var newProductOrderId = await mockHandler.Handle(mockProductOrder, CancellationToken.None);

            // Assert
            Assert.Equal(0, _petShopContext.ProductOrders.Count());
            Assert.Equal(Guid.Empty, newProductOrderId);
        }

        [Fact]
        public async void DeleteProductOrderCommand_DeletesProductOrder()
        {
            // Arrange

            var createCustomer = _fixture.Build<CreateCustomerCommand>().Create();
            var customerId =
                await new CreateCustomerCommandHandler(_petShopContext).Handle(createCustomer, CancellationToken.None);

            var createProduct = _fixture.Build<CreateProductCommand>().Create();
            var productId =
                await new CreateProductCommandHandler(_petShopContext).Handle(createProduct, CancellationToken.None);

            var createProductOrderHandler = new CreateProductsOrderCommandHandler(_petShopContext);
            var newProductOrderId = await createProductOrderHandler.Handle(new CreateProductOrderCommand
            {
                Quantity = 3,
                CustomerId = customerId,
                ProductId = productId
            }, CancellationToken.None);

            // Act
            var mockHandler = new DeleteProductOrderCommandHandler(_petShopContext);
            var result = await mockHandler.Handle(
                new DeleteProductOrderCommand {Id = newProductOrderId}, CancellationToken.None);

            // Assert
            var listOfProductOrders =
                await new ListProductOrdersQueryHandler(_petShopContext).Handle(new ListProductOrdersQuery(),
                    CancellationToken.None);

            Assert.True(result);
            Assert.Empty(listOfProductOrders);
        }
    }
}