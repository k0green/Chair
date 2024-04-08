using Chair.BLL.BusinessLogic.Account;
using Chair.BLL.BusinessLogic.Chat;
using Chair.BLL.BusinessLogic.ExecutorProfile;
using Chair.BLL.BusinessLogic.ExecutorService;
using Chair.BLL.BusinessLogic.Message;
using Chair.BLL.BusinessLogic.Minio;
using Chair.BLL.BusinessLogic.Order;
using Chair.BLL.BusinessLogic.Review;
using Chair.BLL.BusinessLogic.ServiceType;
using Chair.DAL.Data.Entities;
using Chair.DAL.Repositories.Base;
using MediatR;

public static class ServiceConfiguration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IBaseWithManyRepository<ProductFile<ExecutorService>>), typeof(BaseWithManyRepository<ProductFile<ExecutorService>>));

        services.AddScoped<IExecutorServiceBusinessLogic, ExecutorServiceBusinessLogic>();
        services.AddScoped<IServiceTypeBusinessLogic, ServiceTypeBusinessLogic>();
        services.AddScoped<IAccountBusinessLogic, AccountBusinessLogic>();
        services.AddScoped<IExecutorProfileBusinessLogic, ExecutorProfileBusinessLogic>();
        services.AddScoped<IReviewBusinessLogic, ReviewBusinessLogic>();
        services.AddScoped<IOrderBusinessLogic, OrderBusinessLogic>();
        services.AddScoped<IChatBusinessLogic, ChatBusinessLogic>();
        services.AddScoped<IMessageBusinessLogic, MessageBusinessLogic>();
        services.AddScoped<IMinioBusinessLogic, MinioBusinessLogic>();

        services.AddScoped<UserInfo>();

        services.AddScoped(typeof(IBaseWithManyRepository<>), typeof(BaseWithManyRepository<>));
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
    }
}