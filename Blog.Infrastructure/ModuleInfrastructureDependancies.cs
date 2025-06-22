using Blog.Infrastructure.Abstracts;
using Blog.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure
{
    public static class ModuleInfrastructureDependancies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
            services.AddTransient<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddTransient<IPostPictureRespository, PostPictureRespository>();
            services.AddTransient<IPostRespository, PostRespository>();
            return services;
        }
    }
}