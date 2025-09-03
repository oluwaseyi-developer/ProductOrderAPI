using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductOrderApi.Application.Features.Products.Command.CreateProduct;
using ProductOrderApi.Application.Features.Products.Command.DeleteProduct;
using ProductOrderApi.Application.Features.Products.Command.UpdateProduct;
using ProductOrderApi.Application.Features.Products.Dtos;
using ProductOrderApi.Application.Features.Products.Queries.GetProductById;
using ProductOrderApi.Application.Features.Products.Queries.GetProducts;

namespace ProductOrderApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IMediator mediator, ILogger<ProductsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            var result = await _mediator.Send(new GetProductsQuery());

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));

            if (result.IsFailure)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto model)
        {
            var command = new CreateProductCommand(model.Name, model.Description, model.Price, model.StockQuantity);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetProduct), new { id = result.Value.Id }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto model)
        {
            var command = new UpdateProductCommand(id, model.Name, model.Description, model.Price);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            if (result.IsFailure)
                return BadRequest(result.Error);

            return NoContent();
        }
    }
}
