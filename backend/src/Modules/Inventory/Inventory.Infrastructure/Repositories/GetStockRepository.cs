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

    public async Task<List<InventoryStock>> GetStockAsync(int companyId, int? warehouseId)
    {
        var query = _context.InventoryStocks
            .Include(s => s.Product)
                .ThenInclude(p => p.Unit)
            .Include(s => s.Warehouse)
            .Where(s => s.CompanyId == companyId)
            .AsQueryable();

        if (warehouseId.HasValue && warehouseId.Value > 0)
        {
            query = query.Where(s => s.WarehouseId == warehouseId);
        }
        
        return await query.ToListAsync();
    }
}