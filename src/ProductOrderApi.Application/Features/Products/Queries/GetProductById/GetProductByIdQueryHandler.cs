using AutoMapper;
using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Features.Products.Dtos;

namespace ProductOrderApi.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
                if (product == null)
                    return Result<ProductDto>.Failure("Product not found");

                return Result<ProductDto>.Success(_mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                return Result<ProductDto>.Failure($"Failed to retrieve product: {ex.Message}");
            }
        }
    }
}
