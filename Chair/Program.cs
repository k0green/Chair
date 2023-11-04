using System.Text;
using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.BusinessLogic.Review;
using Chair.BLL.BusinessLogic.ServiceType;
using Chair.BLL.Commons;
using Chair.BLL.Extensions.FluentValidation;
using Chair.BLL.Extensions.MediatR;
using Chair.DAL.Data;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.Contact;
using Chair.DAL.Repositories.ExecutorProfile;
using Chair.DAL.Repositories.ExecutorService;
using Chair.DAL.Repositories.Image;
using Chair.DAL.Repositories.Order;
using Chair.DAL.Repositories.Review;
using Chair.DAL.Repositories.ServiceType;
using Chair.Infrastructure;
using Chair.Middllewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "your_issuer",
        ValidAudience = "your_audience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_32_bytes_here")),
    };
});

builder.Services.AddDistributedMemoryCache(); // Добавляет распределенный кэш для сессий
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30); // Установите таймаут сессии по своему усмотрению
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Добавление параметра авторизации Bearer Token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Введите Bearer токен в следующем формате: Bearer {ваш_токен}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddAutoMapper(typeof(AppMappingProfile));
builder.Services.RegisterMediatr()
    .RegisterMediatrValidationPipeline()
    .RegisterMediatrTransactionPipeline();
builder.Services.RegisterFluentValidationValidators();
// Add services to the container.

builder.Services.AddScoped<IExecutorServiceBusinessLogic, ExecutorServiceBusinessLogic>();
builder.Services.AddScoped<IExecutorServiceRepository, ExecutorServiceRepository>();

builder.Services.AddScoped<IServiceTypeBusinessLogic, ServiceTypeBusinessLogic>();
builder.Services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();

builder.Services.AddScoped<IAccountBusinessLogic, AccountBusinessLogic>();

builder.Services.AddScoped<IExecutorProfileBusinessLogic, ExecutorProfileBusinessLogic>();
builder.Services.AddScoped<IExecutorProfileRepository, ExecutorProfileRepository>();

builder.Services.AddScoped<IImageRepository, ImageRepository>();

builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.AddScoped<IReviewBusinessLogic, ReviewBusinessLogic>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

builder.Services.AddScoped<IOrderBusinessLogic, OrderBusinessLogic>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<UserInfo>();

builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .WithOrigins("http://localhost:3000") // Замените этот URL на ваш фронтенд
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSession();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.MapHub<NotificationHub>("/notificationHub");

app.UseCors("CorsPolicy");

app.Run();
