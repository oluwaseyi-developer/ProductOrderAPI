using MediatR;
using Microsoft.Extensions.Logging;
using ProductOrderApi.Application.Common.Interfaces.Services;

namespace ProductOrderApi.Application.Features.Orders.Events
{
    public class OrderPlacedEventHandler : INotificationHandler<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedEventHandler> _logger;
        private readonly IEmailService _emailService;

        public OrderPlacedEventHandler(ILogger<OrderPlacedEventHandler> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task Handle(OrderPlacedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order {OrderId} placed by user {UserId} with total amount {TotalAmount}",
                notification.OrderId, notification.UserId, notification.TotalAmount);

            // Send confirmation email (in a real application)
            try
            {
                await _emailService.SendOrderConfirmationAsync(notification.UserId, notification.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send order confirmation email for order {OrderId}", notification.OrderId);
            }
        }
    }
}
