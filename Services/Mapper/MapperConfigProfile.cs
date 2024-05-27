using AutoMapper;
using Repositories.DTO;
using Repositories.Entities;
using Services.BusinessModels.EventCategoryModels;
using Services.BusinessModels.EventProductsModel;
using Services.BusinessModels.EventModels;
using Services.BusinessModels.UserModels;

namespace Services.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<User, UserDetailsModel>()
           .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null && src.Gender == true ? "Male" : "Female"))
           .ReverseMap();

            CreateMap<UserUpdateModel, User>().
            ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToLower() == "male")).ReverseMap()
           .ReverseMap();

            CreateMap<EventModel, Event>()
                .ReverseMap();

            CreateMap<EventCategory, EventCategoryModel>()
                .ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? (string)null));

            CreateMap<EventProduct, EventProductDetailModel>() .ReverseMap();
            CreateMap<EventProduct, EventProductUpdateModel>().ReverseMap();
            CreateMap<EventProduct, EventProductDetailDTO>().ReverseMap();

            CreateMap<ProductInPackage, ProductInPackageDTO>().ReverseMap();
            CreateMap<EventPackage, EventPackageDetailDTO>().ReverseMap();
        }
    }
}
