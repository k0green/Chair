using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.BusinessLogic.ServiceType;
using Chair.BLL.Extensions.FluentValidation;
using Chair.BLL.Extensions.MediatR;
using Chair.DAL.Data;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.Contact;
using Chair.DAL.Repositories.ExecutorProfile;
using Chair.DAL.Repositories.ExecutorService;
using Chair.DAL.Repositories.Image;
using Chair.DAL.Repositories.ServiceType;
using Chair.Infrastructure;
using Chair.Middllewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
