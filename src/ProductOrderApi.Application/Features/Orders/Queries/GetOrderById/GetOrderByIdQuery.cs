using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Features.Orders.Dtos;

namespace ProductOrderApi.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<Result<OrderDto>>
    {
        public int Id { get; set; }

        public GetOrderByIdQuery(int id)
        {
            Id = id;
        }
    }
}
