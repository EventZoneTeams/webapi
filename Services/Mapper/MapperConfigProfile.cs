using AutoMapper;

namespace Services.Mapper
{
    public class MapperConfigProfile : Profile
    {
        public MapperConfigProfile()
        {
            /*
            CreateMap<AccountDetailsModel, User>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
           .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToLower() == "male"))
           // Chuyển đổi Guid sang string
           .ReverseMap();
            */
        }
    }
}
