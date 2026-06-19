using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchases.Application.Interfaces.ExternalServices;
using Purchases.Application.Interfaces.Repositories;
using Purchases.Infrastructure.ExternalServices;
using Purchases.Infrastructure.Persistence;
using Purchases.Infrastructure.Persistence.Repositories;

namespace Purchases.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPurchaseInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PurchaseDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), 
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "purchases"))
                .UseSnakeCaseNamingConvention()
            );
        
        services.AddHttpClient<IInventoryHttpClient, InventoryHttpClient>(client =>
        {
            var inventoryUrl = configuration["INVENTORY_API_URL"];
            if (inventoryUrl != null) client.BaseAddress = new Uri(inventoryUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.AddScoped<IPurchasesRepository, PurchasesRepository>();

        return services;
    }
}