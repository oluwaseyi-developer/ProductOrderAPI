using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Features.Products.Dtos;

namespace ProductOrderApi.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<Result<IEnumerable<ProductDto>>> { }
}
