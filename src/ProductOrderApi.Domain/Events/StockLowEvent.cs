namespace ProductOrderApi.Domain.Events
{
    public class StockLowEvent : DomainEvent
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
