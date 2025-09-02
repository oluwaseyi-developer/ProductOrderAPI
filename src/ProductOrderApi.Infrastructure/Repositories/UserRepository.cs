using Microsoft.EntityFrameworkCore;
using ProductOrderApi.Application.Common.Interfaces.Repositories;
using ProductOrderApi.Domain.Entities;
using ProductOrderApi.Infrastructure.Data;

namespace ProductOrderApi.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _dbSet
                .AnyAsync(u => u.Email == email);
        }
    }
}
