using AutoMapper;
using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Features.Orders.Dtos;

namespace ProductOrderApi.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetOrderWithItemsAsync(request.Id);
                if (order == null)
                    return Result<OrderDto>.Failure("Order not found");

                return Result<OrderDto>.Success(_mapper.Map<OrderDto>(order));
            }
            catch (Exception ex)
            {
                return Result<OrderDto>.Failure($"Failed to retrieve order: {ex.Message}");
            }
        }
    }
}
