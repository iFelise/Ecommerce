using AutoMapper;
using LIBRARY.Shared.DTO.ProductDTO;
using LIBRARY.Shared.Entity;

namespace BACK_END.Mapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // De DTO -> Entidad
            CreateMap<ProductCreateDto, Product>()
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore()); // porque se sube aparte

            // De Entidad -> DTO de respuesta
            CreateMap<Product, ProductResponseDto>();
            CreateMap<ProductImage, ProductImageDto>();
        }
    }
}
