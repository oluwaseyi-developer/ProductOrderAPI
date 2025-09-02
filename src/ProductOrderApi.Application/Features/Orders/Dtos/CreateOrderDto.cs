using ProductOrderApi.Application.Common.DTOs;

namespace ProductOrderApi.Application.Features.Orders.Dtos
{
    public class CreateOrderDto
    {
        public List<OrderItemRequest> OrderItems { get; set; } = new();
    }
}
