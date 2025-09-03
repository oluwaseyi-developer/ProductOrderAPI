using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProductOrderApi.Application.Features.Products.Dtos;
using ProductOrderApi.Application.Features.Users.Dtos;
using ProductOrderApi.Infrastructure.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ProductOrderApi.API.IntegrationTests
{
    [Collection("Database collection")]
    public class ProductsControllerTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _context;
        private readonly CustomWebApplicationFactory _factory;
        private string _adminToken;

        public ProductsControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _context = factory.Context;
            _factory = factory;
        }

        public async Task InitializeAsync()
        {
            // Login as admin to get token
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new
            {
                Email = "admin@example.com",
                Password = "Admin123!"
            });

            var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
            _adminToken = authResponse.Token;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _adminToken);
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task GetProducts_ReturnsProducts()
        {
            // Act
            var response = await _client.GetAsync("/api/products");

            // Assert
            response.EnsureSuccessStatusCode();
            var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            Assert.NotNull(products);
            Assert.NotEmpty(products);
        }

        [Fact]
        public async Task CreateProduct_WithValidData_ReturnsCreatedProduct()
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
            response.EnsureSuccessStatusCode();
            var createdProduct = await response.Content.ReadFromJsonAsync<ProductDto>();
            Assert.NotNull(createdProduct);
            Assert.Equal(product.Name, createdProduct.Name);
            Assert.Equal(product.Price, createdProduct.Price);
        }

        [Fact]
        public async Task CreateProduct_WithoutAuth_ReturnsUnauthorized()
        {
            // Arrange
            var clientWithoutAuth = _client;
            clientWithoutAuth.DefaultRequestHeaders.Authorization = null;

            var product = new
            {
                Name = "New Product",
                Description = "New Description",
                Price = 29.99m,
                StockQuantity = 50
            };

            // Act
            var response = await clientWithoutAuth.PostAsJsonAsync("/api/products", product);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }

    // CustomWebApplicationFactory for integration tests
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ApplicationDbContext Context { get; private set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Replace database with in-memory
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context
                using var scope = sp.CreateScope();
                var scopedServices = scope.ServiceProvider;
                Context = scopedServices.GetRequiredService<ApplicationDbContext>();

                // Ensure the database is created
                Context.Database.EnsureCreated();
            });
        }

        public async Task InitializeAsync()
        {
            // Seed the database with test data
            await SeedData.Initialize(Services);
        }

        public new Task DisposeAsync()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
            return Task.CompletedTask;
        }
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<CustomWebApplicationFactory>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
