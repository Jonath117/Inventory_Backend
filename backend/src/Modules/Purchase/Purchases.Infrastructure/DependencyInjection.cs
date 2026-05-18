using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchases.Application.Interfaces.Repositories;
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
                    x => x.MigrationsHistoryTable("__ef_migrations_history", "public"))
                .UseSnakeCaseNamingConvention()
            );

        services.AddScoped<IPurchasesRepository, PurchasesRepository>();

        return services;
    }
}