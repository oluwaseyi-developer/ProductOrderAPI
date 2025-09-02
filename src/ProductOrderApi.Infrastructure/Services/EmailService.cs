using Microsoft.Extensions.Logging;
using ProductOrderApi.Application.Common.Interfaces.Services;

namespace ProductOrderApi.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task SendOrderConfirmationAsync(string userId, int orderId)
        {
            _logger.LogInformation("Sending order confirmation email for order {OrderId} to user {UserId}", orderId, userId);
            
            return Task.CompletedTask;
        }

        public Task SendLowStockAlertAsync(int productId, string productName, int remainingStock)
        {
            _logger.LogWarning("Sending low stock alert for product {ProductName} (ID: {ProductId}). Stock: {RemainingStock}",
                productName, productId, remainingStock);
            
            return Task.CompletedTask;
        }
    }
}
