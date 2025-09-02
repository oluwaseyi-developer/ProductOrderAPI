using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Features.Products.Dtos;

namespace ProductOrderApi.Application.Features.Products.Command.CreateProduct
{
    public class CreateProductCommand : IRequest<Result<ProductDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public CreateProductCommand(string name, string description, decimal price, int stockQuantity)
        {
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
        }
    }  
}
