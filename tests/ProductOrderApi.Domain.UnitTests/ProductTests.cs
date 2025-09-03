using ProductOrderApi.Domain.Entities;
using ProductOrderApi.Domain.Enums;

namespace ProductOrderApi.Domain.UnitTests
{
    public class ProductTests
    {
        [Fact]
        public void CreateProduct_WithValidData_ShouldSucceed()
        {
            // Arrange & Act
            var product = new Product("Test Product", "Test Description", 10.99m, 100);

            // Assert
            Assert.Equal("Test Product", product.Name);
            Assert.Equal("Test Description", product.Description);
            Assert.Equal(10.99m, product.Price);
            Assert.Equal(100, product.StockQuantity);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CreateProduct_WithInvalidName_ShouldThrowException(string name)
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new Product(name, "Description", 10.99m, 100));
        }

        [Fact]
        public void DecreaseStock_WithValidQuantity_ShouldUpdateStock()
        {
            // Arrange
            var product = new Product("Test Product", "Test Description", 10.99m, 100);

            // Act
            product.DecreaseStock(20);

            // Assert
            Assert.Equal(80, product.StockQuantity);
        }

        [Fact]
        public void DecreaseStock_WithInsufficientStock_ShouldThrowException()
        {
            // Arrange
            var product = new Product("Test Product", "Test Description", 10.99m, 10);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => product.DecreaseStock(20));
        }

        [Fact]
        public void IncreaseStock_WithValidQuantity_ShouldUpdateStock()
        {
            // Arrange
            var product = new Product("Test Product", "Test Description", 10.99m, 100);

            // Act
            product.IncreaseStock(50);

            // Assert
            Assert.Equal(150, product.StockQuantity);
        }
    }
}
