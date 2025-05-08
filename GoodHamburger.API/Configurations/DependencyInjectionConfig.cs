using GoodHamburger.Application.Interfaces;
using GoodHamburger.Application.Services;
using GoodHamburger.Data.Repositories;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Services;

namespace GoodHamburger.API.Configurations;

public static class DependencyInjectionConfig
{
    /// <summary>
    /// Registers application services and repositories in the dependency injection container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> used to register the services.</param>
    /// <returns>The <see cref="IServiceCollection"/> with the registered services.</returns>
    /// <remarks>
    /// This method registers various scoped services, including repositories for orders and products,
    /// business logic services such as <see cref="DiscountService"/>, and application services like 
    /// <see cref="IProductService"/> and <see cref="IOrderService"/>.
    /// </remarks>
    public static IServiceCollection ResolveDependencies(this IServiceCollection services) 
    {
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<DiscountService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }
}
