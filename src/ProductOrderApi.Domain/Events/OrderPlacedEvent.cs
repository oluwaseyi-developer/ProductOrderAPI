namespace ProductOrderApi.Domain.Events
{
    public class OrderPlacedEvent : DomainEvent
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
