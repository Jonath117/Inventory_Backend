using Inventory.Application.Services;
using Inventory.Domain.Interfaces.IServices;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddInventoryApplication(this IServiceCollection services)
    {
        services.AddScoped<IAdjustmentService, AdjustmentService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IGetProductKardexService, GetProductKardexService>();
        services.AddScoped<IGetStockService, GetStockService>();
        services.AddScoped<ILookUpService, LookUpService>();
        services.AddScoped<IMovementService, MovementService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IUnitService, UnitService>();
        
        return services;
    }
}