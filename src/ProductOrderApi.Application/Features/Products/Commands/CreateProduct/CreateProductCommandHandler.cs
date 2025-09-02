using AutoMapper;
using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Features.Products.Command.CreateProduct;
using ProductOrderApi.Application.Features.Products.Dtos;
using ProductOrderApi.Domain.Entities;

namespace ProductOrderApi.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingProduct = await _unitOfWork.ProductRepository.GetProductByNameAsync(request.Name);
                if (existingProduct != null)
                    return Result<ProductDto>.Failure("Product with this name already exists");

                var product = new Product(request.Name, request.Description, request.Price, request.StockQuantity);
                await _unitOfWork.ProductRepository.AddAsync(product);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<ProductDto>.Success(_mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                return Result<ProductDto>.Failure($"Failed to create product: {ex.Message}");
            }
        }
    }
}
