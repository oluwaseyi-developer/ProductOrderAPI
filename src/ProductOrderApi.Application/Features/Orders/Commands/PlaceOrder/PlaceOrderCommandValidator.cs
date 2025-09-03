using FluentValidation;
using ProductOrderApi.Application.Features.Orders.Command.PlaceOrder;

namespace ProductOrderApi.Application.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("Order must contain at least one item");

            RuleForEach(x => x.OrderItems)
                .SetValidator(new OrderItemRequestValidator());
        }
    }
}
