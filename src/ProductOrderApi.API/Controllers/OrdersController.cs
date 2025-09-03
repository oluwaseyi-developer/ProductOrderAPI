using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductOrderApi.Application.Features.Orders.Command.PlaceOrder;
using ProductOrderApi.Application.Features.Orders.Dtos;
using ProductOrderApi.Application.Features.Orders.Queries.GetOrderById;
using ProductOrderApi.Application.Features.Orders.Queries.GetOrders;
using System.Security.Claims;

namespace ProductOrderApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _mediator.Send(new GetOrdersQuery(userId));

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id));

            if (result.IsFailure)
                return NotFound(result.Error);

            // Ensure user can only access their own orders (unless admin)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && result.Value.UserId != userId)
                return Forbid();

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> PlaceOrder(CreateOrderDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var command = new PlaceOrderCommand(userId, model.OrderItems);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetOrder), new { id = result.Value.Id }, result.Value);
        }
    }
}
