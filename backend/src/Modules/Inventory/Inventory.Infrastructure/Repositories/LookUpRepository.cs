using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class LookUpRepository : ILookUpRepository
{
    private readonly ApplicationDbContext _context;
    
    public LookUpRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductLookUpDto>> GetProductsForDropdownAsync(int companyId)
    {
        return await _context.Products
            .Where(p => p.CompanyId == companyId && p.IsActive)
            .Select(p => new ProductLookUpDto
            {
                Id = p.Id,
                Sku = p.Sku,
                Name = p.Name,
            }).ToListAsync();
    }

    public async Task<IEnumerable<WarehouseLookUpDto>> GetWarehouseForDropdownAsync(int companyId)
    {
        return await _context.Warehouses
            .Where(w => w.CompanyId == companyId && w.IsActive)
            .Select(w => new WarehouseLookUpDto
            {
                Id = w.Id,
                Name =  w.Name,
            }).ToListAsync();
    }
}