using System;
using System.Linq;
using System.Threading;
using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Services.Customers.Commands.CreateCustomerCommand;
using Services.Customers.Queries.ListCustomersQuery;
using Services.Data;
using Services.Models;
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
            var listOfCustomers = _fixture.Build<Customer>().CreateMany(numberOfCustomersToCreate);

            await _petShopContext.Customers.AddRangeAsync(listOfCustomers);
            await _petShopContext.SaveChangesAsync();

            // Act
            var mockHandler = new ListCustomersQueryHandler(_petShopContext);

            var mockHandlerResult = await mockHandler.Handle(new ListCustomersQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(numberOfCustomersToCreate, mockHandlerResult.Count());
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
    }
}
