using MediatR;
using Microsoft.Extensions.Logging;
using ProductOrderApi.Application.Common.Interfaces.Services;


namespace ProductOrderApi.Application.Features.Products.Events
{
    public class StockLowEventHandler : INotificationHandler<StockLowEvent>
    {
        private readonly ILogger<StockLowEventHandler> _logger;
        private readonly IEmailService _emailService;

        public StockLowEventHandler(ILogger<StockLowEventHandler> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Handle(StockLowEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Low stock alert for product {ProductName} (ID: {ProductId}). Remaining stock: {RemainingStock}",
                notification.ProductName, notification.ProductId, notification.RemainingStock);

            // Send low stock notification (in a real application)
            try
            {
                await _emailService.SendLowStockAlertAsync(notification.ProductId, notification.ProductName, notification.RemainingStock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send low stock alert for product {ProductId}", notification.ProductId);
            }
        }
    }
}
