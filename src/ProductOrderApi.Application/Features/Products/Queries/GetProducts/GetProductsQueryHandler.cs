using AutoMapper;
using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Features.Products.Dtos;

namespace ProductOrderApi.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<ProductDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                return Result<IEnumerable<ProductDto>>.Success(_mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<ProductDto>>.Failure($"Failed to retrieve products: {ex.Message}");
            }
        }
    }
}
