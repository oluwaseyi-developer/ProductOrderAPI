using MediatR;
using ProductOrderApi.Domain.Events;

namespace ProductOrderApi.Application.Features.Orders.Events
{
    public class OrderPlacedEvent : DomainEvent, INotification
    {
        public int OrderId { get; }
        public string UserId { get; }
        public decimal TotalAmount { get; }

        public OrderPlacedEvent(int orderId, string userId, decimal totalAmount)
        {
            OrderId = orderId;
            UserId = userId;
            TotalAmount = totalAmount;
        }
    }
}
