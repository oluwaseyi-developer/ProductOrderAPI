using AutoMapper;
using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Features.Orders.Dtos;

namespace ProductOrderApi.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, Result<IEnumerable<OrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(request.UserId);
                return Result<IEnumerable<OrderDto>>.Success(_mapper.Map<IEnumerable<OrderDto>>(orders));
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<OrderDto>>.Failure($"Failed to retrieve orders: {ex.Message}");
            }
        }
    }
}
