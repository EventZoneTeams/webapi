using AutoMapper;
using Domain.Entities;
using Repositories.Models;
using Repositories.Models.ImageDTOs;
using Services.DTO.EventCampaignDTOs;
using Services.DTO.EventCategoryDTOs;
using Services.DTO.EventDTOs;
using Services.DTO.EventFeedbackModel;
using Services.DTO.EventOrderDTOs;
using Services.DTO.EventProductsModel;
using Services.DTO.NotificationDTOs;
using Services.DTO.UserModels;
using Services.DTO.WalletDTOs;

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

            CreateMap<EventDTO, EventResponseDTO>()
                .ReverseMap();

            CreateMap<Event, EventResponseDTO>()
                .ReverseMap();

            CreateMap<EventCategory, EventCategoryDTO>()
                .ReverseMap()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? (string)null));

            CreateMap<EventCategory, EventCategoryResponseDTO>()
                .ReverseMap();

            CreateMap<EventProduct, EventProductDetailModel>().ReverseMap();
            CreateMap<EventProduct, EventProductUpdateModel>().ReverseMap();
            CreateMap<EventProduct, EventProductDetailDTO>().ReverseMap();

            CreateMap<EventCampaign, EventCampaignDTO>().ReverseMap();

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
        }
    }
}