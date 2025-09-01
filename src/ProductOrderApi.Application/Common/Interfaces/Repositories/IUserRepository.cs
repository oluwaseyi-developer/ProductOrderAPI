using ProductOrderApi.Domain.Entities;


namespace ProductOrderApi.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
    }
}
