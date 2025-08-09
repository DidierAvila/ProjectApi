using AutoMapper;
using Business.Customers.Commands;
using Business.Customers.Handlers;
using Business.Logs;
using DataAccess.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestApi.Customers
{
    public class CancelCustomerHandlerTests
    {
        private readonly Mock<ICustomerRepositoy> _customerRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<CancelCustomerCommandHandler>> _loggerMock = new();
        private readonly Mock<ILogService> _logServiceMock = new();

        [Fact]
        public async Task CancelCustomer_Success()
        {
            // Arrange
            var handler = new CancelCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CancelCustomerCommand { CustomerId = 1 };
            var customer = new Customer { CustomerId = 1, Name = "Test Customer", Status = EntityStatus.Active };
            var cancelledCustomer = new Customer { CustomerId = 1, Name = "Test Customer", Status = EntityStatus.Cancelled };
            var expectedDto = new CustomerDto { CustomerId = 1, Name = "Test Customer", Status = EntityStatus.Cancelled };

            _customerRepoMock.Setup(r => r.GetByID(1, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
            _customerRepoMock.Setup(r => r.Update(It.IsAny<Customer>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(expectedDto);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(EntityStatus.Cancelled);
            result.CustomerId.Should().Be(1);
            _customerRepoMock.Verify(r => r.Update(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancelCustomer_Fails_WhenCustomerNotFound()
        {
            // Arrange
            var handler = new CancelCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CancelCustomerCommand { CustomerId = 999 };

            _customerRepoMock.Setup(r => r.GetByID(999, It.IsAny<CancellationToken>())).ReturnsAsync((Customer)null!);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Messages.Should().Contain("No se encontró el cliente con ID 999");
            _customerRepoMock.Verify(r => r.Update(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CancelCustomer_Fails_WhenCustomerAlreadyCancelled()
        {
            // Arrange
            var handler = new CancelCustomerCommandHandler(_customerRepoMock.Object, _mapperMock.Object, _loggerMock.Object, _logServiceMock.Object);
            var command = new CancelCustomerCommand { CustomerId = 1 };
            var customer = new Customer { CustomerId = 1, Name = "Test Customer", Status = EntityStatus.Cancelled };

            _customerRepoMock.Setup(r => r.GetByID(1, It.IsAny<CancellationToken>())).ReturnsAsync(customer);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Messages.Should().Contain("ya está cancelado");
            _customerRepoMock.Verify(r => r.Update(It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}