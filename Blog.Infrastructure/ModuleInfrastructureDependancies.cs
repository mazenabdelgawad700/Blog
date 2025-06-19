using Microsoft.Extensions.DependencyInjection;

namespace Blog.Infrastructure
{
    public static class ModuleInfrastructureDependancies
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            //services.AddTransient<IProductRepository, ProductRepository>();
            return services;
        }
    }
}