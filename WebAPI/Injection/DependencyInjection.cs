using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Commons;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interface;
using Services.Mapper;
using Services.Services;
using Services.Services.VnPayConfig;
using System.Diagnostics;
using WebAPI.MiddleWares;

namespace WebAPI.Injection
{
    public static class DependencyInjection // Chỉ cần một class tồn tại trong project unchanged
    {
        public static IServiceCollection ServicesInjection(this IServiceCollection services, IConfiguration configuration)
        {
            // CONNECT TO DATABASE
            services.AddDbContext<StudentEventForumDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LocalDB"));
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

            // add unitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}