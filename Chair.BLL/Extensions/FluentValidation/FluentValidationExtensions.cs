using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Chair.BLL.Extensions.FluentValidation
{
    public static class FluentValidationExtensions
    {
        public static IServiceCollection RegisterFluentValidationValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
