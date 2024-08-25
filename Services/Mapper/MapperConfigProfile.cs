using AutoMapper;
using EventZone.Domain.DTOs.BookedTicketDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardColumnDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardLabelDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskDTOs;
using EventZone.Domain.DTOs.EventBoardDTOs.EventBoardTaskLabelDTOs;
using EventZone.Domain.DTOs.EventCampaignDTOs;
using EventZone.Domain.DTOs.EventCategoryDTOs;
using EventZone.Domain.DTOs.EventDonationDTOs;
using EventZone.Domain.DTOs.EventDTOs;
using EventZone.Domain.DTOs.EventFeedbackDTOs;
using EventZone.Domain.DTOs.EventOrderDTOs;
using EventZone.Domain.DTOs.EventPackageDTOs;
using EventZone.Domain.DTOs.EventProductDTOs;
using EventZone.Domain.DTOs.ImageDTOs;
using EventZone.Domain.DTOs.NotificationDTOs;
using EventZone.Domain.DTOs.TicketDTOs;
using EventZone.Domain.DTOs.UserDTOs;
using EventZone.Domain.DTOs.WalletDTOs;
using EventZone.Domain.Entities;

namespace EventZone.Services.Mapper
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

            CreateMap<User, UserDTO>()
          .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender != null && src.Gender == true ? "Male" : "Female"))
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

            CreateMap<Event, EventCreateDTO>()
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
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl ?? null));

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
            CreateMap<EventOrder, EventOrderBookedTicketDTO>().ReverseMap();

            CreateMap<Notification, NotificationDTO>().ReverseMap();

            CreateMap<EventDonation, EventDonationCreateDTO>().ReverseMap();
            CreateMap<EventDonation, EventDonationDetailDTO>().ReverseMap();

            //Ticket
            CreateMap<EventTicket, EventTicketDetailDTO>().ReverseMap();
            CreateMap<EventTicket, EventTicketDTO>().ReverseMap();
            CreateMap<EventTicket, EventTicketUpdateDTO>().ReverseMap();

            // EventBoard mappings
            CreateMap<EventBoard, EventBoardResponseDTO>()
                .ReverseMap();

            CreateMap<EventBoardCreateDTO, EventBoard>()
                .ReverseMap();

            CreateMap<EventBoardUpdateDTO, EventBoard>()
                .ReverseMap();

            // EventBoardColumn mappings
            CreateMap<EventBoardColumn, EventBoardColumnDTO>()
                .ReverseMap();

            CreateMap<EventBoardColumn, EventBoardColumnDTO>()
                .ForMember(dest => dest.EventBoardTasks, opt => opt.MapFrom(src => src.EventBoardTasks));

            CreateMap<EventBoardColumnCreateDTO, EventBoardColumn>()
                .ReverseMap();

            CreateMap<EventBoardColumnUpdateDTO, EventBoardColumn>()
                .ReverseMap();

            // EventBoardLabel mappings
            CreateMap<EventBoardLabel, EventBoardLabelDTO>()
                .ReverseMap();
            CreateMap<EventBoardLabelCreateDTO, EventBoardLabel>()
                .ReverseMap();

            CreateMap<EventBoardLabelUpdateDTO, EventBoardLabel>()
                .ReverseMap();

            // EventBoardTaskLabel mappings
            CreateMap<EventBoardTaskLabel, EventBoardTaskLabelDTO>()
                .ReverseMap();
            CreateMap<EventBoardTaskLabelCreateDTO, EventBoardTaskLabel>()
                .ReverseMap();

            CreateMap<EventBoardTaskLabelUpdateDTO, EventBoardTaskLabel>()
                .ReverseMap();

            // Mapping for EventBoardLabelAssignment to EventBoardLabelAssignmentDTO
            CreateMap<EventBoardLabelAssignment, EventBoardLabelAssignmentDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.EventBoardLabel.Name))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.EventBoardLabel.Color))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EventBoardLabelId));

            // Mapping for EventBoard to EventBoardResponseDTO
            CreateMap<EventBoard, EventBoardResponseDTO>()
                .ForMember(dest => dest.EventBoardLabels, opt => opt.MapFrom(src => src.EventBoardLabelAssignments))
                .ForMember(dest => dest.EventBoardColumns, opt => opt.MapFrom(src => src.EventBoardColumns))
                .ForMember(dest => dest.EventBoardTaskLabels, opt => opt.MapFrom(src => src.EventBoardTaskLabels));

            // Mapping for EventBoardTask to EventBoardTaskResponseDTO
            CreateMap<EventBoardTask, EventBoardTaskResponseDTO>()
                .ForMember(dest => dest.EventBoardTaskLabels, opt => opt.MapFrom(src => src.EventBoardTaskLabelAssignments.Select(x => x.EventBoardTaskLabel)))
                .ForMember(dest => dest.EventBoardTaskAssignments, opt => opt.MapFrom(src => src.EventBoardTaskAssignments));

            CreateMap<EventBoardTaskLabelAssignment, EventBoardTaskLabelDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EventBoardTaskLabelId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.EventBoardTaskLabel.Name))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.EventBoardTaskLabel.Color));

            CreateMap<EventBoardLabelAssignment, EventBoardLabelAssignmentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EventBoardLabelId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.EventBoardLabel.Name))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.EventBoardLabel.Color));

            CreateMap<EventBoardTaskAssignment, EventBoardTaskAssignmentDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId));

            //BookedTicket
            CreateMap<BookedTicket, BookedTicketDTO>().ReverseMap();
            CreateMap<BookedTicket, BookedTicketDetailDTO>().ReverseMap();
            CreateMap<BookedTicket, BookedTicketUpdateDTO>().ReverseMap();
        }
    }
}