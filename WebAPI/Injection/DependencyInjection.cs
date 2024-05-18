using AutoMapper;
using Repositories.Commons;
using Repositories.Interfaces;
using Services.Interface;
using Services.Mapper;
using Services.Services;
using System.Diagnostics;
using WebAPI.MiddleWares;

namespace WebAPI.Injection
{
    public static class DependencyInjection // Chỉ cần một class tồn tại trong project unchanged
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services)
        {
            //sign up for middleware
            services.AddSingleton<GlobalExceptionMiddleware>();
            services.AddTransient<PerformanceTimeMiddleware>();
            //services.AddScoped<AccountStatusMiddleware>(); // sử dụng ClaimsIdentity nên dùng Addscoped theo request
            //others
            services.AddSingleton<Stopwatch>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(MapperConfigProfile).Assembly);
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentTime, CurrentTime>();

            return services;
        }

        public static IServiceCollection AddWebAPIServices(this IServiceCollection services)
        {
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddHttpContextAccessor();

            return services;
        }

    }
}
