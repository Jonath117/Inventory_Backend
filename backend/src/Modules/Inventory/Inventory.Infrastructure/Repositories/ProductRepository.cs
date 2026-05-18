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


    public async Task<IEnumerable<Product>> GetProductsAsync(int companyId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Where(p => p.CompanyId == companyId)
            .ToListAsync();
    }
    
    public async Task<Product?> GetByProductCenAsync(int companyId, string productCen)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.ProductCen == productCen);
    }
    public async Task<bool> ExistsBySkuAsync(int companyId, string sku, string? excludeProductCen = null)
    {
        var query = _context.Products.Where(p => p.CompanyId == companyId && p.Sku == sku);

        if (!string.IsNullOrEmpty(excludeProductCen))
        {
            query = query.Where(p => p.ProductCen != excludeProductCen);
        }

        return await query.AnyAsync();
    }

    public async Task<Product> AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Product?> GetByIdAsync(int companyId, int productId)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.Id == productId);
    }

    public async Task<(int Id, string Name, string UnitName)> GetProductInfoByCenAsync(int companyId, string productCen)
    {
        throw new NotImplementedException();
    }
}