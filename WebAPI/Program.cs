using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repositories;
using Repositories.Entities;
using Services.Interface;
using Services.Services;
using Services.ViewModels.EmailModels;
using System.Text;
using WebAPI.Injection;
using WebAPI.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container, CONFIG FOR IGNORE NULL OBJECT IN JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//CONFIG FOR JWT AUTHENTICATION ON SWAGGER
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo { Title = "Student Event Forum", Version = "v.1.0" });
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put Bearer + your token in the box below",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    config.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
{
        {
            jwtSecurityScheme, Array.Empty<string>()
        }
});
});

// CONNECT TO DATABASE
builder.Services.AddDbContext<StudentEventForumDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"));
});

//SETUP INJECTION SERVICE
builder.Services.AddInfrastructuresService();

//SETUP SERVICE IDENTITY: Allow non alphanumeric
builder.Services.AddIdentityCore<User>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
})
    .AddRoles<Role>()
    .AddEntityFrameworkStores<StudentEventForumDbContext>();
//ADD AUTHENTICATION - CONFIG FOR JWT
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
builder.Services.AddAuthorization();

//==========================
//Loi cc j zay, xiu check lai
// ADD REPOSITORY
//builder.Services.AddScoped<IAccountRepository, AccountRepository>();
// Add services to the container.
//builder.Services.AddWebAPIServices();
//==========================


//CORS - Set Policy
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicyDevelopement", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
    });
    opt.AddPolicy("CorsPolicyProduction", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://student-event-forum.netlify.app");
    });
});

//ADD EMAIL CONFIG
var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
                   .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();


// SCOPE FOR MIGRATION
// explain: The CreateScope method creates a new scope. The scope is a way to manage the lifetime of objects in the container.
var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<StudentEventForumDbContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await DBInitializer.Initialize(context, userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An problem occurred during migration!");
}

//Show SWAGGER UI
app.UseSwagger();
app.UseSwaggerUI(config =>
{
    // Always keep token after reload or refresh browser
    config.SwaggerEndpoint("/swagger/v1/swagger.json", "Student Event Forum API v.01");
    config.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
});

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<PerformanceTimeMiddleware>();
app.UseHttpsRedirection();

// USE AUTHENTICATION, AUTHORIZATION
app.UseAuthentication();
app.UseAuthorization();

// USE MIDDLEWARE
app.UseMiddleware<AccountStatusMiddleware>();

app.MapControllers();

//USE CORS
app.UseCors("CorsPolicyDevelopement");
app.UseCors("CorsPolicyProduction");

app.Run();
