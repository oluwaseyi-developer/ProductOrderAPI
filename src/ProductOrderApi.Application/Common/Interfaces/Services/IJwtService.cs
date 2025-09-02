using ProductOrderApi.Domain.Entities;

namespace ProductOrderApi.Application.Common.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
