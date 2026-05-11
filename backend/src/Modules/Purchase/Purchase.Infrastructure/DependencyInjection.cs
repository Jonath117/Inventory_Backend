using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Purchase.Infrastructure.Persistence;

namespace Purchase.Infrastructure;

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

        return services;
    }
}