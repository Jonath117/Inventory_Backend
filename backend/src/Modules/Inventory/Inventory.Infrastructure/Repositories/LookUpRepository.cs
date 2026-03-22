using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class LookUpRepository : ILookUpRepository
{
    private readonly InventoryDbContext _context;
    
    public LookUpRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetProductsForDropdownAsync(int companyId)
    {
        return await _context.Products
            .Where(p => p.CompanyId == companyId && p.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Warehouse>> GetWarehouseForDropdownAsync(int companyId)
    {
        return await _context.Warehouses
            .Where(w => w.CompanyId == companyId && w.IsActive)
            .ToListAsync();
    }
}