using AutoMapper;
using Repositories.DTO;
using Repositories.Entities;
using Services.BusinessModels.EventCategoryModels;

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

            CreateMap<EventCategoryModel, EventCategory>()
                .ReverseMap();
        }
    }
}
