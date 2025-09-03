using AutoMapper;
using MediatR;
using Moq;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Features.Orders.Command.PlaceOrder;
using ProductOrderApi.Application.Features.Orders.Commands.PlaceOrder;
using ProductOrderApi.Application.Features.Orders.Dtos;
using ProductOrderApi.Domain.Entities;

namespace ProductOrderApi.Application.UnitTests.Handlers
{
    public class PlaceOrderCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PlaceOrderCommandHandler _handler;

        public PlaceOrderCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _mediatorMock = new Mock<IMediator>();
            _handler = new PlaceOrderCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidRequest_ShouldPlaceOrder()
        {
            // Arrange
            var product = new Product("Test Product", "Description", 10m, 100);
            var orderItems = new List<OrderItemRequest> { new() { ProductId = 1, Quantity = 2 } };
            var command = new PlaceOrderCommand("user123", orderItems);

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetProductsByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new[] { product });
            _unitOfWorkMock.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.OrderRepository.AddAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.ProductRepository.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<OrderDto>(It.IsAny<Order>()))
                .Returns(new OrderDto { Id = 1, TotalAmount = 20m });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(20m, result.Value.TotalAmount);
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInsufficientStock_ShouldFail()
        {
            // Arrange
            var product = new Product("Test Product", "Description", 10m, 1);
            var orderItems = new List<OrderItemRequest> { new() { ProductId = 1, Quantity = 2 } };
            var command = new PlaceOrderCommand("user123", orderItems);

            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.ProductRepository.GetProductsByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new[] { product });
            _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Contains("Insufficient stock", result.Error);
            _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        }
    }
}
