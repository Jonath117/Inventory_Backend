using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Interfaces;
using Sales.Application.Interfaces.ExternalServices;
using Sales.Infrastructure.ExternalServices;
using Sales.Infrastructure.Persistence;
using Sales.Infrastructure.Persistence.Repositories;

namespace Sales.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddSalesInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SalesDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), 
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "sales"))
                .UseSnakeCaseNamingConvention();
        });
        
        services.AddScoped<ISalesRepository, SalesRepository>();
        services.AddHttpClient<IInventoryHttpClient, InventoryHttpClient>(client =>
        {
            client.BaseAddress = new Uri("http://localhost:5224/"); 
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        return services;
    }
}