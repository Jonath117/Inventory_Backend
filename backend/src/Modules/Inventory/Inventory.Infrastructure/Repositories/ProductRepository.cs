using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryDbContext _context;

    public ProductRepository(InventoryDbContext context)
    {
        _context = context;
    }


    public async Task<List<Product>> GetProductsAsync(int companyId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Where(p => p.CompanyId == companyId)
            .ToListAsync();
    }
}