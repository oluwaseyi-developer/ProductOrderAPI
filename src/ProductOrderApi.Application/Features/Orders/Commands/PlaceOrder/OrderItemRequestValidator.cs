using FluentValidation;
using ProductOrderApi.Application.Common.DTOs;

namespace ProductOrderApi.Application.Features.Orders.Commands.PlaceOrder
{
    public class OrderItemRequestValidator : AbstractValidator<OrderItemRequest>
    {
        public OrderItemRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be valid");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1");
        }
    }
}
