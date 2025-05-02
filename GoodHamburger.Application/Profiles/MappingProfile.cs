using AutoMapper;
using GoodHamburger.Application.DTOs.Product;
using GoodHamburger.Domain.Models;

namespace GoodHamburger.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => s.Type.ToString()));

            CreateMap<ProductDTO, Product>()
                .ForMember(d => d.Type, opt => opt.MapFrom(s => Enum.Parse<ProductType>(s.Type)));            
        }
    }
}
