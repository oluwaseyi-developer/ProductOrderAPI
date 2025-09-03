using ProductOrderApi.Domain.Entities;
using ProductOrderApi.Domain.Enums;
using Xunit;

namespace ProductOrderApi.Domain.UnitTests
{
    public class OrderTests
    {
        [Fact]
        public void CreateOrder_WithValidItems_ShouldCalculateTotal()
        {
            // Arrange
            var product1 = new Product("Product 1", "Description 1", 10m, 100);
            var product2 = new Product("Product 2", "Description 2", 20m, 50);

            var items = new List<OrderItem>
        {
            new OrderItem(product1, 2), // 20
            new OrderItem(product2, 1)  // 20
        };

            // Act
            var order = new Order("user123", items);

            // Assert
            Assert.Equal(40m, order.TotalAmount);
            Assert.Equal(OrderStatus.Pending, order.Status);
            Assert.Equal(2, order.OrderItems.Count);
        }

        [Fact]
        public void CompleteOrder_FromPending_ShouldUpdateStatus()
        {
            // Arrange
            var product = new Product("Product", "Description", 10m, 100);
            var order = new Order("user123", new[] { new OrderItem(product, 1) });

            // Act
            order.CompleteOrder();

            // Assert
            Assert.Equal(OrderStatus.Completed, order.Status);
        }

        [Fact]
        public void CancelOrder_FromPending_ShouldUpdateStatus()
        {
            // Arrange
            var product = new Product("Product", "Description", 10m, 100);
            var order = new Order("user123", new[] { new OrderItem(product, 1) });

            // Act
            order.CancelOrder();

            // Assert
            Assert.Equal(OrderStatus.Cancelled, order.Status);
        }

        [Fact]
        public void CancelOrder_FromCompleted_ShouldThrowException()
        {
            // Arrange
            var product = new Product("Product", "Description", 10m, 100);
            var order = new Order("user123", new[] { new OrderItem(product, 1) });
            order.CompleteOrder();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.CancelOrder());
        }
    }
}
