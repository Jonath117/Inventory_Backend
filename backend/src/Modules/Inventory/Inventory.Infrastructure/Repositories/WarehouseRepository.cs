using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly InventoryDbContext _context;

    public WarehouseRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<(int Id, string Name)> GetInfoByCenAsync(int companyId, string warehouseCen)
    {
        var warehouse = await _context.Warehouses
            .AsNoTracking()
            .Where(w => w.CompanyId == companyId && w.WarehouseCen == warehouseCen)
            .Select(w => new { w.Id, w.Name })
            .FirstOrDefaultAsync();

        if (warehouse == null) return (0, string.Empty);
        return (warehouse.Id, warehouse.Name);
    }

    public async Task<IEnumerable<Warehouse>> GetAllAsync(int companyId)
    {
        return await _context.Warehouses
            .Where(w => w.CompanyId == companyId)
            .ToListAsync();
    }

    public async Task<Warehouse?> GetByCenAsync(int companyId, string warehouseCen)
    {
        return await _context.Warehouses
            .FirstOrDefaultAsync(w => w.CompanyId == companyId && w.WarehouseCen == warehouseCen);
    }
}