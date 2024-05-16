using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Entities;
using Repositories.Interfaces;
using Repositories.Repositories;
using Services.Interface;
using Services.Services;
using Services.ViewModels.EmailModels;
using System.Text;
using WebAPI.Injection;
using WebAPI.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//CONFIG FOR JWT AUTHENTICATION ON SWAGGER
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Template with Identity", Version = "v.1.0" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
//CONFIG FOR IGNORE NULL OBJECT
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
        });
//SETUP FOR MIDDLEWARE
//builder.Services.AddSingleton<GlobalExceptionMiddleware>();
//builder.Services.AddTransient<PerformanceTimeMiddleware>();
//builder.Services.AddScoped<AccountStatusMiddleware>(); // sử dụng ClaimsIdentity nên dùng Addscoped theo request
builder.Services.AddInfrastructuresService();
//SETUP SERVICE
builder.Services.AddIdentity<Account, IdentityRole>()
    .AddEntityFrameworkStores<TemplateDbContext>().AddDefaultTokenProviders();
/*
builder.Services.AddDbContext<TemplateDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"));
});
*/
//connect db with fly.io
string connString;
if (builder.Environment.IsDevelopment())
    connString = builder.Configuration.GetConnectionString("LocalDB");
else
{
    // Use connection string provided at runtime by FlyIO.
    var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    // Parse connection URL to connection string for SQL Server
    connUrl = connUrl.Replace("sqlserver://", string.Empty);
    var userPass = connUrl.Split("@")[0];
    var hostPortDb = connUrl.Split("@")[1];
    var hostPort = hostPortDb.Split("/")[0];
    var db = hostPortDb.Split("/")[1];
    var user = userPass.Split(":")[0];
    var pass = userPass.Split(":")[1];
    var host = hostPort.Split(":")[0];
    var port = hostPort.Split(":")[1];

    //Server=host.docker.internal;Database=SWD-Student-Event-Forum;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True
    connString = $"Server=host.docker.internal;Database=SWD-Student-Event-Forum;User Id=sa;Password=yourStrong(!)Password;TrustServerCertificate=True";
}

builder.Services.AddDbContext<TemplateDbContext>(opt =>
{
    opt.UseSqlServer(connString);
});



builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddWebAPIServices();

//CONFIG FOR JWT
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
        };
    });

//ADD EMAIL CONFIG
builder.Services.Configure<IdentityOptions>(
    opts => opts.SignIn.RequireConfirmedEmail = true
    );

var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
                   .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

//builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));

builder.Services.AddScoped<IEmailService, EmailService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(
    c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CAPYBARA API v.01")
    );
//}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<PerformanceTimeMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseMiddleware<AccountStatusMiddleware>();

app.MapControllers();

app.Run();
