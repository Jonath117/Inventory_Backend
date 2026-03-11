using Microsoft.EntityFrameworkCore;
using WebApp1.Domain.DTOs;
using WebApp1.Domain.Interfaces;
using WebApp1.Infrastructure.Data;

namespace WebApp1.Business.Services;

public class GetStockService: IGetStockService
{
    private readonly ApplicationDbContext _context;

    public GetStockService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<StockDto>> GetCurrentStockAsync(int companyId, int? warehouseId = null)
    {
        var query = _context.InventoryStocks
            .Include(s => s.Product)
            .Include(s => s.Warehouse)
            .Where(s => s.CompanyId == companyId )
            .AsQueryable();

        if (warehouseId.HasValue && warehouseId.Value > 0)
        {
            query = query.Where(s => s.WarehouseId ==  warehouseId.Value);
        }
        
        var stockList = await query
            .Select(s => new StockDto
            {
                ProductId = s.ProductId,
                Sku = s.Product!.Sku,
                ProductName = s.Product.Name,
                WarehouseName = s.Warehouse!.Name,
                CurrentStock = s.CurrentStock,
                UnitOfMeasure = s.Product.UnitOfMeasure,
                MinStockAlert = s.Product.MinStockAlert,
                LastUpdated =  s.LastUpdated,
            })
            .ToListAsync();
        
        return stockList;
    }
    
}