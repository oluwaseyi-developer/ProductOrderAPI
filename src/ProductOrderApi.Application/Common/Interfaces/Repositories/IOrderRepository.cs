using ProductOrderApi.Domain.Entities;

namespace ProductOrderApi.Application.Common.Interfaces.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> GetOrderWithItemsAsync(int orderId);
    }
}
