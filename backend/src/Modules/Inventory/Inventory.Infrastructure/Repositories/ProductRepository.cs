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

    public async Task<bool> ExistsBySkuAsync(int companyId, string sku, int? excludeProductId = null)
    {
        var query = _context.Products.Where(p => companyId == p.CompanyId && sku.ToLower() == p.Sku.ToLower());

        if (excludeProductId.HasValue)
        {
            query = query.Where(p => p.Id != excludeProductId.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> GetByIdAsync(int companyId, int productId)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.Id == productId);
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
}