using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.Services;
using GoodHamburger.Data.Repositories;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Services;

namespace GoodHamburger.API.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services) 
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<DiscountService>();
            services.AddScoped<IProductService, ProductService>();

            return services;
        }
    }
}
