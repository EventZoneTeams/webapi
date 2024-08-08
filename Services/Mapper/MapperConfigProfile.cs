using AutoMapper;
using Domain.DTOs.EventCampaignDTOs;
using Domain.DTOs.EventCategoryDTOs;
using Domain.DTOs.EventDonationDTOs;
using Domain.DTOs.EventDTOs;
using Domain.DTOs.EventFeedbackDTOs;
using Domain.DTOs.EventOrderDTOs;
using Domain.DTOs.EventPackageDTOs;
using Domain.DTOs.EventProductDTOs;
using Domain.DTOs.ImageDTOs;
using Domain.DTOs.NotificationDTOs;
using Domain.DTOs.UserDTOs;
using Domain.DTOs.WalletDTOs;
using Domain.Entities;

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

            CreateMap<EventDTO, Event>()
                .ReverseMap();

            CreateMap<EventCreateDTO, Event>()
                .ReverseMap();


            CreateMap<EventDTO, EventResponseDTO>()
                .ReverseMap();

            CreateMap<Event, EventResponseDTO>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new LocationResponseDTO
                {
                    Latitude = src.Latitude,
                    Longitude = src.Longitude,
                    Display = src.LocationDisplay,
                    Note = src.LocationNote
                }))
                .ReverseMap()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.Longitude))
                .ForMember(dest => dest.LocationDisplay, opt => opt.MapFrom(src => src.Location.Display))
                .ForMember(dest => dest.LocationNote, opt => opt.MapFrom(src => src.Location.Note));

            CreateMap<EventCategory, EventCategoryDTO>()
                .ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? (string)null));

            CreateMap<EventCategory, EventCategoryResponseDTO>()
                .ReverseMap();

            CreateMap<EventProduct, EventProductDetailDTO>().ReverseMap();
            CreateMap<EventProduct, EventProductUpdateDTO>().ReverseMap();
            CreateMap<EventProduct, EventProductDetailDTO>().ReverseMap();

            CreateMap<EventCampaign, EventCampaignDTO>().ReverseMap();
            CreateMap<EventCampaign, EventCampaignStaticticDTO>().ReverseMap();
            CreateMap<EventCampaign, EventCampaignUpdateDTO>().ReverseMap();

            CreateMap<ProductInPackage, ProductInPackageDTO>().ReverseMap();
            CreateMap<EventPackage, EventPackageDetailDTO>().ReverseMap();
            CreateMap<Wallet, WalletResponseDTO>().ReverseMap();

            CreateMap<ProductImage, ImageReturnDTO>().ReverseMap();
            CreateMap<TransactionResponsesDTO, Transaction>().ReverseMap();

            CreateMap<EventFeedbackDetailModel, EventFeedback>().ReverseMap();

            //Order
            CreateMap<EventOrder, EventOrderReponseDTO>().ReverseMap();
            CreateMap<EventOrderDetail, EventOrderDetailsReponseDTO>().ReverseMap();
            CreateMap<EventOrderDetail, CreateEventOrderDetailsReponseDTO>().ReverseMap();

            CreateMap<Notification, NotificationDTO>().ReverseMap();

            CreateMap<EventDonation, EventDonationCreateDTO>().ReverseMap();
            CreateMap<EventDonation, EventDonationDetailDTO>().ReverseMap();
        }
    }
}