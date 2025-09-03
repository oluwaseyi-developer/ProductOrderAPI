using AutoMapper;
using MediatR;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Common.Interfaces;
using ProductOrderApi.Application.Features.Orders.Command.PlaceOrder;
using ProductOrderApi.Application.Features.Orders.Dtos;
using ProductOrderApi.Application.Features.Orders.Events;
using ProductOrderApi.Application.Features.Products.Events;
using ProductOrderApi.Domain.Entities;


namespace ProductOrderApi.Application.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PlaceOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Result<OrderDto>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Get all products in the order
                var productIds = request.OrderItems.Select(oi => oi.ProductId).ToList();
                var products = (await _unitOfWork.ProductRepository.GetProductsByIdsAsync(productIds))
                    .ToDictionary(p => p.Id);

                // Validate products and stock
                var orderItems = new List<OrderItem>();
                foreach (var item in request.OrderItems)
                {
                    if (!products.TryGetValue(item.ProductId, out var product))
                        return Result<OrderDto>.Failure($"Product with ID {item.ProductId} not found");

                    if (product.StockQuantity < item.Quantity)
                        return Result<OrderDto>.Failure($"Insufficient stock for product {product.Name}");

                    var orderItem = new OrderItem(product, item.Quantity);
                    orderItems.Add(orderItem);
                    product.DecreaseStock(item.Quantity);

                    await _unitOfWork.ProductRepository.UpdateAsync(product);

                    if (product.StockQuantity < 10) // Threshold
                    {
                        await _mediator.Publish(
                            new StockLowEvent(product.Id, product.Name, product.StockQuantity),
                            cancellationToken
                        );
                    }
                }

                // Create and save order
                var order = new Order(request.UserId, orderItems);
                order.CompleteOrder();

                await _unitOfWork.OrderRepository.AddAsync(order);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync();

                // Raise order placed event
                await _mediator.Publish(new OrderPlacedEvent(order.Id, order.UserId, order.TotalAmount), cancellationToken);

                return Result<OrderDto>.Success(_mapper.Map<OrderDto>(order));
            }
            catch (Exception ex)
            {
                return Result<OrderDto>.Failure($"Failed to place order: {ex.Message}");
            }
            finally
            {
                // Always rollback if transaction wasn't committed
                await _unitOfWork.RollbackTransactionAsync();
            }
        }

    }

}