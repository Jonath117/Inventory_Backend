using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class GetStockRepository: IGetStockRepository
{
    private readonly InventoryDbContext _context;
    
    public GetStockRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InventoryStock>> GetCurrentStockAsync(int companyId, string? productCen, string? warehouseCen)
    {
        var query = _context.InventoryStocks
            .Include(s => s.Product)
                .ThenInclude(p => p.Unit)
            .Include(s => s.Warehouse)
            .Where(s => s.CompanyId == companyId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(warehouseCen))
        {
            query = query.Where(s => s.Warehouse != null && s.Warehouse.WarehouseCen == warehouseCen);
        }

        if (!string.IsNullOrEmpty(productCen))
        {
            query = query.Where(s => s.Product!.ProductCen == productCen);
        }
        
        return await query.ToListAsync();
    }
}