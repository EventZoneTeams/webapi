using AutoMapper;
using Repositories.DTO;
using Repositories.Entities;
using Services.BusinessModels.EventCategoryModels;
using Services.BusinessModels.EventProductsModel;

namespace Services.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<User, UserDetailsModel>()
           .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null && src.Gender == true ? "Male" : "Female"))
           .ReverseMap();

            CreateMap<EventModel, Event>()
                .ReverseMap();

            CreateMap<EventCategory, EventCategoryModel>()
                .ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? (string)null));

            CreateMap<EventProduct, EventProductDetailModel>() .ReverseMap();
        }
    }
}
