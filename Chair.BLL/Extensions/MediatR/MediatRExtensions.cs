using Microsoft.Extensions.DependencyInjection;
using MediatR.NotificationPublishers;
using System.Reflection;
using Chair.BLL.Commons.MidiatRPipeline;
using MediatR;

namespace Chair.BLL.Extensions.MediatR
{
    public static class MediatRExtensions
    {
        public static IServiceCollection RegisterMediatr(this IServiceCollection services)
        {
            services.AddMediatR(x =>
            {
                x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                x.NotificationPublisher = new TaskWhenAllPublisher();
            });

            return services;
        }

        public static IServiceCollection RegisterMediatrValidationPipeline(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static IServiceCollection RegisterMediatrTransactionPipeline(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            return services;
        }
    }
}
