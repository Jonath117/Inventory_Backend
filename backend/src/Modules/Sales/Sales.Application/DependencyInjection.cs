using Microsoft.Extensions.DependencyInjection;

namespace Sales.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddSalesApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });
        return services;
    }
}