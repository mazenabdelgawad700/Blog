using Microsoft.Extensions.DependencyInjection;

namespace Blog.Service
{
    public static class ModuleIServiceDependancies
    {
        public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
        {
            //services.AddTransient<IApplicationUserService, ApplicationUserService>();
            return services;
        }
    }
}
