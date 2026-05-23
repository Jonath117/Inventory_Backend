using Microsoft.Extensions.DependencyInjection;

namespace Purchases.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddPurchaseApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });
        
        return services;
    }
}