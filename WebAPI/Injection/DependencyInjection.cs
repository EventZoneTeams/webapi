using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Commons;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interface;
using Services.Mapper;
using Services.Services;
using System.Diagnostics;
using WebAPI.MiddleWares;

namespace WebAPI.Injection
{
    public static class DependencyInjection // Chỉ cần một class tồn tại trong project unchanged
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, IConfiguration configuration)
        {
            // CONNECT TO DATABASE
            services.AddDbContext<StudentEventForumDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LocalDB"));
            });

            //sign up for middleware
            services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddTransient<PerformanceTimeMiddleware>();
            services.AddScoped<UserStatusMiddleware>(); // sử dụng ClaimsIdentity nên dùng Addscoped theo request
            //others
            services.AddSingleton<Stopwatch>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperConfigProfile).Assembly);
            services.AddScoped<IClaimsService, ClaimsService>();
            // add repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<IEventRepository, EventRepository>();

            // add signInManager
            services.AddScoped<SignInManager<User>>();

            // add unitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentTime, CurrentTime>();

            // add services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventService, EventService>();

            return services;
        }
    }
}
