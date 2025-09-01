using ProductOrderApi.Domain.Enums;

namespace ProductOrderApi.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string UserId { get; private set; }
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public decimal TotalAmount { get; private set; }

        private readonly List<OrderItem> _orderItems = new();
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        protected Order() { }

        public Order(string userId, IEnumerable<OrderItem> orderItems)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            OrderDate = DateTime.UtcNow;
            Status = OrderStatus.Pending;

            foreach (var item in orderItems)
            {
                AddOrderItem(item);
            }

            CalculateTotal();
        }

        public void AddOrderItem(OrderItem item)
        {
            _orderItems.Add(item);
            CalculateTotal();
        }

        public void CompleteOrder()
        {
            if (Status != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be completed");

            Status = OrderStatus.Completed;
        }

        public void CancelOrder()
        {
            if (Status == OrderStatus.Completed)
                throw new InvalidOperationException("Completed orders cannot be cancelled");

            Status = OrderStatus.Cancelled;
        }

        private void CalculateTotal()
        {
            TotalAmount = _orderItems.Sum(item => item.GetTotal());
        }
    }


}