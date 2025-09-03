using Microsoft.Extensions.DependencyInjection;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Domain.Entities;

namespace ProductOrderApi.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<ApplicationDbContext>();
            var unitOfWork = services.GetRequiredService<IUnitOfWork>();

            // Check if database already has data
            if (context.Products.Any() || context.Users.Any())
                return;

            // Seed admin user
            var adminPassword = BCrypt.Net.BCrypt.HashPassword("Admin123!");
            var adminUser = new User("admin@example.com", "Admin", "User", adminPassword);
            adminUser.AddRole("Admin");
            await context.Users.AddAsync(adminUser);

            // Seed regular user
            var userPassword = BCrypt.Net.BCrypt.HashPassword("User123!");
            var regularUser = new User("user@example.com", "Regular", "User", userPassword);
            regularUser.AddRole("Customer");
            await context.Users.AddAsync(regularUser);

            // Seed products
            var products = new List<Product>
        {
            new Product("Laptop", "High-performance laptop", 999.99m, 50),
            new Product("Smartphone", "Latest smartphone", 699.99m, 100),
            new Product("Headphones", "Noise-cancelling headphones", 199.99m, 75),
            new Product("Monitor", "27-inch 4K monitor", 399.99m, 30),
            new Product("Keyboard", "Mechanical keyboard", 89.99m, 120)
        };

            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
    }
}

