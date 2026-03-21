using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _context;
    
    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CompanyExistsAsync(int companyId)
    {
        return await _context.Companies.AnyAsync(c => c.Id == companyId);
    }

    public async Task<int> GetTotalProductsAsync(int companyId)
    {
        return await _context.Products
            .CountAsync(p => p.CompanyId == companyId && p.IsActive);
    }

    public async Task<int> GetTotalWarehousesAsync(int companyId)
    {
        return await _context.Warehouses
            .CountAsync(w => w.CompanyId == companyId && w.IsActive);
    }

    public async Task<decimal> GetTotalStockAsync(int companyId)
    {
        return await _context.InventoryStocks
            .Where(i => i.CompanyId == companyId)
            .SumAsync(s => s.CurrentStock);
    }

    public async Task<int> GetLowStockAlertsAsync(int companyId)
    {
        return await _context.InventoryStocks
            .Include(s => s.Product)
            .Where(s => s.CompanyId == companyId &&
                        s.CurrentStock <= s.Product.MinStockAlert)
            .CountAsync();
    }
}