using AutoMapper;
using EventZone.Domain.Entities;
using EventZone.Repositories;
using EventZone.Repositories.Commons;
using EventZone.Repositories.Interfaces;
using EventZone.Repositories.Repositories;
using EventZone.Services.Interface;
using EventZone.Services.Mapper;
using EventZone.Services.Services;
using EventZone.Services.Services.VnPayConfig;
using EventZone.WebAPI.MiddleWares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EventZone.WebAPI.Injection
{
    public static class DependencyInjection // Chỉ cần một class tồn tại trong project unchanged
    {
        public static IServiceCollection ServicesInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // CONNECT TO DATABASE
            services.AddDbContext<StudentEventForumDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisCacheDB");
            });

            //sign up for middleware
            services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddTransient<PerformanceTimeMiddleware>();
            services.AddScoped<UserStatusMiddleware>(); // sử dụng ClaimsIdentity nên dùng Addscoped theo request
            //others
            services.AddScoped<ICurrentTime, CurrentTime>();
            services.AddSingleton<Stopwatch>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperConfigProfile).Assembly);
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddScoped<IPayOSService, PayOSService>();
            services.AddScoped<IRedisService, RedisService>();
            // add repositories
            services.AddScoped<IEventFeedbackRepository, EventFeedbackRepository>();
            services.AddScoped<IEventPackageRepository, EventPackageRepository>();
            services.AddScoped<IEventProductRepository, EventProductRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();// ****
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IEventOrderRepository, EventOrderRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IEventCampaignRepository, EventCampaignRepository>();
            services.AddScoped<IEventDonationRepository, EventDonationRepository>();
            services.AddScoped<IEventTicketRepository, EventTicketRepository>();
            services.AddScoped<IEventBoardRepository, EventBoardRepository>();
            services.AddScoped<IEventBoardTaskLabelRepository, EventBoardTaskLabelRepository>();
            services.AddScoped<IEventBoardLabelRepository, EventBoardLabelRepository>();
            services.AddScoped<IEventBoardColumnRepository, EventBoardColumnRepository>();
            services.AddScoped<IEventBoardTaskRepository, EventBoardTaskRepository>();
            services.AddScoped<IAttendeeRepository, AttendeeRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IEventStaffRepository, EventStaffRepository>();

            // add generic repositories
            services.AddScoped<IGenericRepository<EventFeedback>, GenericRepository<EventFeedback>>();
            services.AddScoped<IGenericRepository<EventPackage>, GenericRepository<EventPackage>>();
            services.AddScoped<IGenericRepository<EventProduct>, GenericRepository<EventProduct>>();
            services.AddScoped<IGenericRepository<Event>, GenericRepository<Event>>();
            services.AddScoped<IGenericRepository<EventCategory>, GenericRepository<EventCategory>>();// ****
            services.AddScoped<IGenericRepository<Wallet>, GenericRepository<Wallet>>();
            services.AddScoped<IGenericRepository<Transaction>, GenericRepository<Transaction>>();
            services.AddScoped<IGenericRepository<EventOrder>, GenericRepository<EventOrder>>();
            services.AddScoped<IGenericRepository<Notification>, GenericRepository<Notification>>();
            services.AddScoped<IGenericRepository<EventCampaign>, GenericRepository<EventCampaign>>();
            services.AddScoped<IGenericRepository<EventDonation>, GenericRepository<EventDonation>>();
            services.AddScoped<IGenericRepository<EventTicket>, GenericRepository<EventTicket>>();
            services.AddScoped<IGenericRepository<EventBoard>, GenericRepository<EventBoard>>();
            services.AddScoped<IGenericRepository<EventBoardTaskLabel>, GenericRepository<EventBoardTaskLabel>>();
            services.AddScoped<IGenericRepository<EventBoardLabel>, GenericRepository<EventBoardLabel>>();
            services.AddScoped<IGenericRepository<EventBoardColumn>, GenericRepository<EventBoardColumn>>();
            services.AddScoped<IGenericRepository<EventBoardTask>, GenericRepository<EventBoardTask>>();
            services.AddScoped<IGenericRepository<BookedTicket>, GenericRepository<BookedTicket>>();
            services.AddScoped<IGenericRepository<ProductImage>, GenericRepository<ProductImage>>();
            services.AddScoped<IGenericRepository<EventStaff>, GenericRepository<EventStaff>>();

            // add signInManager
            services.AddScoped<SignInManager<User>>();
            // add services
            services.AddScoped<IEventFeedbackService, EventFeedbackService>();
            services.AddScoped<IEventPackageService, EventPackageService>();
            services.AddScoped<IEventProductService, EventProductService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventCategoryService, EventCategoryService>(); // ****
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IEventOrderService, EventOrderService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEventCampaignService, EventCampaignService>();
            services.AddScoped<IEventDonationService, EventDonationService>();
            services.AddScoped<IEventTicketService, EventTicketService>();
            services.AddScoped<IEventBoardService, EventBoardService>();
            services.AddScoped<IEventBoardTaskLabelService, EventBoardTaskLabelService>();
            services.AddScoped<IEventBoardLabelService, EventBoardLabelService>();
            services.AddScoped<IEventBoardColumnService, EventBoardColumnService>();
            services.AddScoped<IEventBoardTaskService, EventBoardTaskService>();
            services.AddScoped<IAttendeeService, AttendeeService>();
            services.AddScoped<IEventStaffService, EventStaffService>();

            // add unitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}