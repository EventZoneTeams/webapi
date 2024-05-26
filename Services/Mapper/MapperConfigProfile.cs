using AutoMapper;
using Repositories.DTO;
using Repositories.Entities;
using Services.BusinessModels.EventCategoryModels;
using Services.BusinessModels.EventModels;

namespace Services.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            CreateMap<UserDetailsModel, User>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
           .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToLower() == "male"))
           // Chuyển đổi Guid sang string
           .ReverseMap();

            CreateMap<EventModel, Event>()
                .ReverseMap();

            CreateMap<EventCategory, EventCategoryModel>()
                .ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? (string)null));
        }
    }
}
