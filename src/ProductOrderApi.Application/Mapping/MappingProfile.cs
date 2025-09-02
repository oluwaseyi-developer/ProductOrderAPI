using AutoMapper;
using ProductOrderApi.Application.Common.DTOs;
using ProductOrderApi.Application.Features.Orders.Dtos;
using ProductOrderApi.Application.Features.Products.Dtos;
using ProductOrderApi.Application.Features.Users.Dtos;
using ProductOrderApi.Domain.Entities;

namespace ProductOrderApi.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

            CreateMap<User, UserDto>();
        }
    }
}
