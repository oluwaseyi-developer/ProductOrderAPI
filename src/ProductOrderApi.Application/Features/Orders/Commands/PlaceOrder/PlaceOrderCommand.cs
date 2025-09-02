using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Features.Orders.Dtos;

namespace ProductOrderApi.Application.Features.Orders.Command.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<Result<OrderDto>>
    {
        public string UserId { get; set; }
        public List<OrderItemRequest> OrderItems { get; set; } = new();

        public PlaceOrderCommand(string userId, List<OrderItemRequest> orderItems)
        {
            UserId = userId;
            OrderItems = orderItems;
        }
    }
}
