using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Features.Orders.Dtos;

namespace ProductOrderApi.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<Result<IEnumerable<OrderDto>>>
    {
        public string UserId { get; set; }

        public GetOrdersQuery(string userId)
        {
            UserId = userId;
        }
    }
}
