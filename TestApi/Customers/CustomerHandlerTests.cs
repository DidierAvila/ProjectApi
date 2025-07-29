using AutoMapper;
using Business.Customers.Commands;
using Business.Customers.Handlers;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestApi.Customers
{
    public class CustomerHandlerTests
    {
        private readonly Mock<ICustomerRepositoy> _customerRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<CreateCustomerCommandHandler>> _loggerMock = new();

        [Fact]
        public async Task CreateCustomer_Success()
        {
            var handler = new CreateCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new CreateCustomerCommand { Name = "Test Customer" };
            _customerRepoMock.Setup(r => r.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null!);
            _mapperMock.Setup(m => m.Map<Customer>(It.IsAny<CreateCustomerCommand>())).Returns(new Customer { Name = "Test Customer" });
            _customerRepoMock.Setup(r => r.Create(It.IsAny<Customer>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { Name = "Test Customer" });
            _mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(new CustomerDto { Name = "Test Customer" });
            var result = await handler.Handle(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.Name.Should().Be("Test Customer");
        }

        [Fact]
        public async Task CreateCustomer_Fails_WhenNameExists()
        {
            var handler = new CreateCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new CreateCustomerCommand { Name = "Existing" };
            _customerRepoMock.Setup(r => r.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { Name = "Existing" });
            await Assert.ThrowsAsync<System.InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreateCustomer_Fails_WhenNameIsEmpty()
        {
            var handler = new CreateCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new CreateCustomerCommand { Name = "" };
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreateCustomer_MapsCorrectly()
        {
            var handler = new CreateCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new CreateCustomerCommand { Name = "MapTest" };
            _customerRepoMock.Setup(r => r.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null!);
            _mapperMock.Setup(m => m.Map<Customer>(It.IsAny<CreateCustomerCommand>())).Returns(new Customer { Name = "MapTest" });
            _customerRepoMock.Setup(r => r.Create(It.IsAny<Customer>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { Name = "Test Customer" });
            _mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(new CustomerDto { Name = "MapTest" });
            var result = await handler.Handle(command, CancellationToken.None);
            result.Name.Should().Be("MapTest");
        }

        [Fact]
        public async Task CreateCustomer_Throws_OnRepositoryException()
        {
            var handler = new CreateCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new CreateCustomerCommand { Name = "RepoError" };
            _customerRepoMock.Setup(r => r.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null!);
            _mapperMock.Setup(m => m.Map<Customer>(It.IsAny<CreateCustomerCommand>())).Returns(new Customer { Name = "RepoError" });
            _customerRepoMock.Setup(r => r.Create(It.IsAny<Customer>(), It.IsAny<CancellationToken>())).ThrowsAsync(new System.Exception("DB Error"));
            await Assert.ThrowsAsync<System.Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreateCustomer_ValidatesNameIsTrimmed()
        {
            var handler = new CreateCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new CreateCustomerCommand { Name = "   Trimmed   " };
            _customerRepoMock.Setup(r => r.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null!);
            _mapperMock.Setup(m => m.Map<Customer>(It.IsAny<CreateCustomerCommand>())).Returns(new Customer { Name = "Trimmed" });
            _customerRepoMock.Setup(r => r.Create(It.IsAny<Customer>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { Name = "Test Customer" });
            _mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(new CustomerDto { Name = "Trimmed" });
            var result = await handler.Handle(command, CancellationToken.None);
            result.Name.Should().Be("Trimmed");
        }

        [Fact]
        public async Task CreateCustomer_Throws_OnNullName()
        {
            var handler = new CreateCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new CreateCustomerCommand { Name = null! };
            await Assert.ThrowsAsync<System.InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreateCustomer_RepositoryCalledOnce()
        {
            var handler = new CreateCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new CreateCustomerCommand { Name = "RepoCall" };
            _customerRepoMock.Setup(r => r.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null!);
            _mapperMock.Setup(m => m.Map<Customer>(It.IsAny<CreateCustomerCommand>())).Returns(new Customer { Name = "RepoCall" });
            _customerRepoMock.Setup(r => r.Create(It.IsAny<Customer>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { Name = "Test Customer" });
            _mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(new CustomerDto { Name = "RepoCall" });
            await handler.Handle(command, CancellationToken.None);
            _customerRepoMock.Verify(r => r.Create(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateCustomer_MapperCalled()
        {
            var handler = new CreateCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new CreateCustomerCommand { Name = "MapCall" };
            _customerRepoMock.Setup(r => r.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null!);
            _mapperMock.Setup(m => m.Map<Customer>(It.IsAny<CreateCustomerCommand>())).Returns(new Customer { Name = "MapCall" });
            _customerRepoMock.Setup(r => r.Create(It.IsAny<Customer>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Customer { Name = "Test Customer" });
            _mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(new CustomerDto { Name = "MapCall" });
            await handler.Handle(command, CancellationToken.None);
            _mapperMock.Verify(m => m.Map<Customer>(It.IsAny<CreateCustomerCommand>()), Times.Once);
        }

        [Fact]
        public async Task CreateCustomer_Throws_OnMapperException()
        {
            var handler = new CreateCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object);
            var command = new CreateCustomerCommand { Name = "MapError" };
            _customerRepoMock.Setup(r => r.Find(It.IsAny<System.Linq.Expressions.Expression<System.Func<Customer, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null!);
            _mapperMock.Setup(m => m.Map<Customer>(It.IsAny<CreateCustomerCommand>())).Throws(new System.Exception("Map error"));
            await Assert.ThrowsAsync<System.Exception>(() => handler.Handle(command, CancellationToken.None));
        }
    }
} 