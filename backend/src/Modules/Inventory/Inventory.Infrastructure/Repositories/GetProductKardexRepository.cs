using Microsoft.EntityFrameworkCore;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;

namespace Inventory.Infrastructure.Repositories;

public class GetProductKardexRepository : IGetProductKardexRepository
{
    private readonly InventoryDbContext _context;
    
    public GetProductKardexRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<InventoryMovement>> GetMovementsAsync(int companyId, int productId, int? warehouseId)
    {
        var query = _context.InventoryMovements
            .Include(m => m.Warehouse)
            .Where(m => m.CompanyId == companyId && m.ProductId == productId)
            .AsQueryable();

        if (warehouseId.HasValue && warehouseId.Value > 0)
        {
            query = query.Where(m => m.WarehouseId == warehouseId.Value);
        }
        return await query
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }
}