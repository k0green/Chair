using Chair.BLL.Commons;
using Chair.BLL.Extensions.FluentValidation;
using Chair.BLL.Extensions.Jobs;
using Chair.BLL.Extensions.MediatR;
using Chair.DAL.Data;
using Chair.DAL.Data.Entities;
using Chair.Infrastructure;
using Chair.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var frontServer = builder.Configuration.GetConnectionString("FrontServer");
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
        IssuerSigningKey = new SymmetricSecurityKey("your_secret_key_32_bytes_here"u8.ToArray()),
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/messageHub"))
            {
                context.Token = accessToken;
            }

            Console.WriteLine($"Access Token: {accessToken}");
            Console.WriteLine($"Request Path: {path}");
            Console.WriteLine($"Context Token: {context.Token}");

            return Task.CompletedTask;
        }
    };

});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

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

builder.Services.RegisterServices();

builder.Services.AddSignalR();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
            .WithOrigins(frontServer)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    q.UsePersistentStore(options =>
    {
        options.UseProperties = true;

        options.UseSqlServer(sqlOptions =>
        {
            sqlOptions.ConnectionString = connectionString;
        });

        options.UseClustering();
        options.UseJsonSerializer();
    });
});


builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});


builder.Services.AddSingleton<MinioClient>(provider =>
{
    IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
    var minioConfig = configuration.GetSection("MinioConfig");

    var minio = new MinioClient()
        .WithEndpoint(minioConfig["Endpoint"])
        .WithCredentials(minioConfig["AccessKey"], minioConfig["SecretKey"])
        .WithSSL(bool.Parse(minioConfig["UseSSL"]))
        .Build();

    return (MinioClient)minio;
});

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
    await scheduler.Start();

    // Вызов метода восстановления задач
    await RestoreScheduledJobs(scheduler);
});

async Task RestoreScheduledJobs(IScheduler scheduler)
{
    // Получаем все задания из любого группы
    var jobKeys = await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

    foreach (var jobKey in jobKeys)
    {
        // Получаем триггеры, связанные с этим заданием
        var triggers = await scheduler.GetTriggersOfJob(jobKey);

        foreach (var trigger in triggers)
        {
            // Проверяем, есть ли следующее время выполнения
            if (trigger.GetNextFireTimeUtc() != null)
            {
                // Добавляем триггер обратно в планировщик
                await scheduler.ScheduleJob(trigger);
            }
        }
    }
}

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
/*
app.UseMiddleware<SecurityMiddleware>();
*/

app.MapControllers();

app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<MessageHub>("/messageHub");

app.UseCors("CorsPolicy");

app.Run();