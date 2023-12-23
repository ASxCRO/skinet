using API.DTO;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(x=>x.ProductBrand, y=>y.MapFrom(x=>x.ProductBrand.Name))
                .ForMember(x=>x.ProductType, y=>y.MapFrom(x=>x.ProductType.Name))
                .ForMember(d=>d.PictureUrl, o=>o.MapFrom<ProductUrlResolver>());
        }
    }
}