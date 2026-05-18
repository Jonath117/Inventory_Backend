using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Inventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<InventoryDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention();
            
        });
        
        services.AddScoped<IAdjustmentRepository, AdjustmentRepository>();
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IGetProductKardexRepository, GetProductKardexRepository>();
        services.AddScoped<IGetStockRepository, GetStockRepository>();
        services.AddScoped<ILookUpRepository, LookUpRepository>();
        services.AddScoped<IMovementRepository, MovementRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<IWarehouseRepository, WarehouseRepository>();
        
        return services;
    }
    
    
}