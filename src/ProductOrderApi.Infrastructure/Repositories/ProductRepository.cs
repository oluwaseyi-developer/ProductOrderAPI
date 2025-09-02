using Microsoft.EntityFrameworkCore;
using ProductOrderApi.Application.Common.Interfaces.Repositories;
using ProductOrderApi.Domain.Entities;
using ProductOrderApi.Infrastructure.Data;

namespace ProductOrderApi.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(IEnumerable<int> productIds)
        {
            return await _dbSet
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<Product?> GetProductByNameAsync(string name)
        {
            return await _dbSet
                .FirstOrDefaultAsync(p => p.Name == name);
        }
    }

}
