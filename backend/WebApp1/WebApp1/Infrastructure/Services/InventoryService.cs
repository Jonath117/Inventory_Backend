using Microsoft.EntityFrameworkCore;
using WebApp1.Core.DTOs;
using WebApp1.Core.Interfaces;
using WebApp1.Infrastructure.Data;

namespace WebApp1.Infrastructure.Services;

public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;

    public InventoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardMetricsAsync(int companyId)
    {
        var companyExists = await _context.Companies.AnyAsync(c => c.Id == companyId);
        if(!companyExists) throw new Exception("La empresa no existe");
        
        var totalProducts = await _context.Products
            .CountAsync(p => p.CompanyId == companyId && p.IsActive);
        
        var totalWarehouses = await _context.Warehouses
            .CountAsync(w => w.CompanyId == companyId && w.IsActive);
        
        var totalStock = await _context.InventoryStocks
            .Where(s => s.CompanyId ==  companyId)
            .SumAsync(s => s.CurrentStock);
        
        var lowStockCount = await _context.InventoryStocks
            .Include(s => s.Product)
            .Where(s => s.CompanyId == companyId && s.CurrentStock <= s.Product.MinStockAlert)
            .CountAsync();

        return new DashboardDto()
        {
            TotalProducts = totalProducts,
            TotalStockQuantity = totalStock,
            TotalWarehouses = totalWarehouses,
            LowStockAlerts = lowStockCount
        };
    }
    
}