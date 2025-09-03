using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProductOrderApi.Domain.Entities;
using ProductOrderApi.Infrastructure.Data;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ProductOrderApi.API.IntegrationTests;

public class BasicIntegrationTests : IAsyncLifetime
{
    private TestServer _server;
    private HttpClient _client;
    private ApplicationDbContext _context;

    public async Task InitializeAsync()
    {
        // Create test server
        var builder = new WebHostBuilder()
            .UseEnvironment("Development")
            .UseStartup<Program>()
            .ConfigureServices(services =>
            {
                // Replace the database with in-memory
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });
            });

        _server = new TestServer(builder);
        _client = _server.CreateClient();

        // Get the DbContext
        var scopeFactory = _server.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Initialize database
        await _context.Database.EnsureCreatedAsync();
        await SeedTestData();
    }

    private async Task SeedTestData()
    {
        // Clear existing data
        _context.Products.RemoveRange(_context.Products);
        _context.Users.RemoveRange(_context.Users);
        _context.Orders.RemoveRange(_context.Orders);
        await _context.SaveChangesAsync();

        // Seed admin user
        var adminPassword = BCrypt.Net.BCrypt.HashPassword("Admin123!");
        var adminUser = new User("admin@example.com", "Admin", "User", adminPassword);
        adminUser.AddRole("Admin");
        await _context.Users.AddAsync(adminUser);

        // Seed regular user
        var userPassword = BCrypt.Net.BCrypt.HashPassword("User123!");
        var regularUser = new User("user@example.com", "Regular", "User", userPassword);
        regularUser.AddRole("Customer");
        await _context.Users.AddAsync(regularUser);

        // Seed products
        var products = new List<Product>
        {
            new Product("Laptop", "High-performance laptop", 999.99m, 50),
            new Product("Smartphone", "Latest smartphone", 699.99m, 100),
            new Product("Headphones", "Noise-cancelling headphones", 199.99m, 75)
        };

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        _server?.Dispose();
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }

    [Fact]
    public async Task GetProducts_ReturnsProducts()
    {
        // Act
        var response = await _client.GetAsync("/api/products");

        // Assert
        response.EnsureSuccessStatusCode();
        var products = await response.Content.ReadFromJsonAsync<List<ProductModel>>();
        Assert.NotNull(products);
        Assert.NotEmpty(products);
    }

    [Fact]
    public async Task CreateProduct_WithoutAuth_ReturnsUnauthorized()
    {
        // Arrange
        var product = new
        {
            Name = "New Product",
            Description = "New Description",
            Price = 29.99m,
            StockQuantity = 50
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/products", product);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new
        {
            Email = "admin@example.com",
            Password = "Admin123!"
        });

        var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        return authResponse!.Token;
    }
}

// Test models
public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public UserModel User { get; set; } = new UserModel();
}

public class UserModel
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}

public class ProductModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}