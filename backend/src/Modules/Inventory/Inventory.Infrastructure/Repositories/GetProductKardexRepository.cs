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

    public async Task<IEnumerable<InventoryMovement>> GetProductKardexAsync(int companyId, string productCen, string? warehouseCen, DateTime? from = null, DateTime? to = null)
    {
        var query = _context.InventoryMovements
            .Include(m => m.Warehouse)
            .Include(m => m.Product)
            .Where(m => m.CompanyId == companyId && m.Product!.ProductCen == productCen)
            .AsQueryable();

        if (!string.IsNullOrEmpty(warehouseCen))
        {
            query = query.Where(m => m.Warehouse!.WarehouseCen == warehouseCen);
        }

        if (from.HasValue)
        {
            query = query.Where(m => m.CreatedAt >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(m => m.CreatedAt <= to.Value);
        }

        return await query
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }
}