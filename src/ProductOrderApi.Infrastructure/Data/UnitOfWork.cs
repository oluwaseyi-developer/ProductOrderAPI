using Microsoft.EntityFrameworkCore.Storage;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Common.Interfaces.Repositories;

namespace ProductOrderApi.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        public IProductRepository ProductRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IUserRepository UserRepository { get; }

        public UnitOfWork(ApplicationDbContext context,
                         IProductRepository productRepository,
                         IOrderRepository orderRepository,
                         IUserRepository userRepository)
        {
            _context = context;
            ProductRepository = productRepository;
            OrderRepository = orderRepository;
            UserRepository = userRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();

            // Dispose repositories if they implement IDisposable
            if (ProductRepository is IDisposable productRepo)
                productRepo.Dispose();
            if (OrderRepository is IDisposable orderRepo)
                orderRepo.Dispose();
            if (UserRepository is IDisposable userRepo)
                userRepo.Dispose();
        }
    }
}
