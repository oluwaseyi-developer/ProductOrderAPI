using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Features.Products.Dtos;

namespace ProductOrderApi.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<Result<ProductDto>>
    {
        public int Id { get; set; }

        public GetProductByIdQuery(int id)
        {
            Id = id;
        }
    }
}
