using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Interfaces;
using Sales.Infrastructure.Persistence;
using Sales.Infrastructure.Persistence.Repositories;

namespace Sales.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSalesInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention();
        });
        
        services.AddScoped<ISalesRepository, SalesRepository>();
        
        return services;
    }
}