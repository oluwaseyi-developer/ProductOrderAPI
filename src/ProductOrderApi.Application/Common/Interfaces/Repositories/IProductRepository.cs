using ProductOrderApi.Domain.Entities;

namespace ProductOrderApi.Application.Common.Interfaces.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> productIds);
        Task<Product> GetProductByNameAsync(string name);
    }
}
