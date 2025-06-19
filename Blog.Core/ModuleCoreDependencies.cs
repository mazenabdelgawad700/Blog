using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Blog.Core
{
    public static class ModuleCoreDependencies
    {
        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            services.AddMediatR(
            cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly())
            );

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
