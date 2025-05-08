using AutoMapper;
using GoodHamburger.Application.DTOs.Order;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Models;

namespace GoodHamburger.Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()));

        CreateMap<ProductDTO, Product>()
            .ForMember(d => d.Type, opt => opt.MapFrom(s => Enum.Parse<ProductType>(s.Type)));

        CreateMap<Order, OrderResponseDTO>();

        CreateMap<OrderItem, OrderItemResponseDTO>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Product.Type.ToString()));
    }
}
