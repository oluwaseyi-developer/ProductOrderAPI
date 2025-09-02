using MediatR;
using ProductOrderApi.Application.Common.DTOs;

namespace ProductOrderApi.Application.Features.Products.Command.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public UpdateProductCommand(int id, string name, string description, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
