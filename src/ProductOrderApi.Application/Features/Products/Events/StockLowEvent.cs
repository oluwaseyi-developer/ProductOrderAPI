using MediatR;
using ProductOrderApi.Domain.Events;

namespace ProductOrderApi.Application.Features.Products.Events
{
    public class StockLowEvent : DomainEvent, INotification
    {
        public int ProductId { get; }
        public string ProductName { get; }
        public int RemainingStock { get; }

        public StockLowEvent(int productId, string productName, int remainingStock)
        {
            ProductId = productId;
            ProductName = productName;
            RemainingStock = remainingStock;
        }
    }
}
