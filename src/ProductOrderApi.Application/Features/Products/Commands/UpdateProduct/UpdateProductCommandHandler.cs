using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Features.Products.Command.UpdateProduct;

namespace ProductOrderApi.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
                if (product == null)
                    return Result.Failure("Product not found");

                product.Update(request.Name, request.Description, request.Price);
                await _unitOfWork.ProductRepository.UpdateAsync(product);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update product: {ex.Message}");
            }
        }
    }
}
