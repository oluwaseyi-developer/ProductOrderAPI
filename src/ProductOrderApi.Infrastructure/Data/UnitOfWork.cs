﻿using Microsoft.EntityFrameworkCore.Storage;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Common.Interfaces.Repositories;
using ProductOrderApi.Infrastructure.Repositories;

namespace ProductOrderApi.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;

        public IProductRepository ProductRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IUserRepository UserRepository { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            ProductRepository = new ProductRepository(context);
            OrderRepository = new OrderRepository(context);
            UserRepository = new UserRepository(context);
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
                _transaction = null!;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction?.RollbackAsync();
            _transaction?.Dispose();
            _transaction = null!;
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }
    }
}
