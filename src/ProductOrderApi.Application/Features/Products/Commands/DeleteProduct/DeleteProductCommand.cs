using MediatR;
using ProductOrderApi.Application.Common.DTOs;

namespace ProductOrderApi.Application.Features.Products.Command.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Result>
    {
        public int Id { get; set; }

        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }
}
