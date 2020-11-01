using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Services.Customers.Commands.CreateCustomerCommand;
using Services.Customers.Commands.DeleteCustomerCommand;
using Services.Customers.Commands.UpdateCustomerInfoCommand;
using Services.Customers.Queries.GetCustomerByIdQuery;
using Services.Customers.Queries.ListCustomersQuery;
using Services.Data;
using Xunit;

namespace MediatrExample.Tests
{
    public class CustomersTests
    {
        private readonly IFixture _fixture;
        private readonly PetShopContext _petShopContext;

        public CustomersTests()
        {
            _fixture = new Fixture();

            var options = new DbContextOptionsBuilder<PetShopContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _petShopContext = new PetShopContext(options);
        }

        [Fact]
        public async void ListCustomersQuery_ReturnsAllCustomers()
        {
            const int numberOfCustomersToCreate = 5;

            // Arrange
            var createCustomers = _fixture.Build<CreateCustomerCommand>()
                .CreateMany(numberOfCustomersToCreate);
            
            var createCustomerCommandHandler = new CreateCustomerCommandHandler(_petShopContext);
            foreach (var customer in createCustomers)
            {
                await createCustomerCommandHandler.Handle(customer, CancellationToken.None);
            }

            // Act
            var mockHandler = new ListCustomersQueryHandler(_petShopContext);
            var mockHandlerResult = 
                await mockHandler.Handle(new ListCustomersQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(numberOfCustomersToCreate, mockHandlerResult.Count());
        }

        [Fact]
        public async void GetCustomerByIdQuery_GetsCustomer()
        {
            // Arrange
            var createCustomer = _fixture.Build<CreateCustomerCommand>().Create();
            var customerId =
                await new CreateCustomerCommandHandler(_petShopContext).Handle(createCustomer, CancellationToken.None);

            // Act
            var mockHandler = new GetCustomerByIdQueryHandler(_petShopContext);
            var customer = 
                await mockHandler.Handle(new GetCustomerByIdQuery {Id = customerId}, CancellationToken.None);

            // Assert
            Assert.NotNull(customer);
            Assert.Equal(customerId, customer.Id);
        }

        [Fact]
        public async void CreateCustomerCommand_CreatesACustomer()
        {
            // Arrange
            var mockCustomer = _fixture.Build<CreateCustomerCommand>().Create();

            // Act
            var createCustomerCommandHandler = new CreateCustomerCommandHandler(_petShopContext);
            var newCustomerId = await createCustomerCommandHandler.Handle(mockCustomer, CancellationToken.None);

            // Assert
            var numberOfCustomers = _petShopContext.Customers.Count();

            Assert.Equal(1, numberOfCustomers);
            Assert.NotEqual(Guid.Empty, newCustomerId);
        }

        [Fact]
        public async void CreateCustomerCommand_EmptyName_DoesNotCreateCustomer()
        {
            // Arrange
            var mockCustomerEmptyFirstName = _fixture.Build<CreateCustomerCommand>()
                .With(p => p.FirstName, string.Empty)
                .Create();

            var mockCustomerEmptyLastName = _fixture.Build<CreateCustomerCommand>()
                .Without(p => p.LastName)
                .Create();

            // Act
            var createCustomerCommandHandler = new CreateCustomerCommandHandler(_petShopContext);

            var newCustomerId1 =
                await createCustomerCommandHandler.Handle(mockCustomerEmptyFirstName, CancellationToken.None);
            var newCustomerId2 =
                await createCustomerCommandHandler.Handle(mockCustomerEmptyLastName, CancellationToken.None);

            // Assert
            var numberOfCustomers = _petShopContext.Customers.Count();

            Assert.Equal(0, numberOfCustomers);
            Assert.Equal(Guid.Empty, newCustomerId1);
            Assert.Equal(Guid.Empty, newCustomerId2);
        }

        [Fact]
        public async void UpdateCustomerInfoCommand_UpdatesCustomerInfo()
        {
            // Arrange
            var createCustomer = _fixture.Build<CreateCustomerCommand>().Create();
            var customerId =
                await new CreateCustomerCommandHandler(_petShopContext).Handle(createCustomer, CancellationToken.None);

            // Act
            var updatedCustomer = _fixture.Build<UpdateCustomerInfoCommand>()
                .With(c => c.Id, customerId)
                .Without(c => c.FirstName)
                .Create();

            var mockHandler = new UpdateCustomerInfoCommandHandler(_petShopContext);
            var result = await mockHandler.Handle(updatedCustomer, CancellationToken.None);

            // Assert
            var customer = await new GetCustomerByIdQueryHandler(_petShopContext)
                .Handle(new GetCustomerByIdQuery{ Id = customerId }, CancellationToken.None);

            Assert.Equal(customer.Id, result.Id);
            Assert.Equal(customer.FirstName, result.FirstName);
            Assert.Equal(customer.LastName, result.LastName);
            Assert.Equal(customer.Address, result.Address);
            Assert.Equal(customer.Email, result.Email);
            Assert.Equal(customer.Phone, result.Phone);
        }

        [Fact]
        public async void DeleteCustomerCommand_DeletesCustomer()
        {
            // Arrange
            var createCustomer = _fixture.Build<CreateCustomerCommand>().Create();
            var customerId =
                await new CreateCustomerCommandHandler(_petShopContext).Handle(createCustomer, CancellationToken.None);

            // Act
            var mockHandler = new DeleteCustomerCommandHandler(_petShopContext);
            var result = await mockHandler.Handle(
                new DeleteCustomerCommand {Id = customerId}, CancellationToken.None);

            // Assert
            var customers = await new ListCustomersQueryHandler(_petShopContext)
                .Handle(new ListCustomersQuery(), CancellationToken.None);

            Assert.True(result);
            Assert.Empty(customers);
        }
    }
}