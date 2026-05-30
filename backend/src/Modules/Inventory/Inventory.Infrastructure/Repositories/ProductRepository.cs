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
        var product = await _context.Products
            .Include(p => p.Unit)
            .AsNoTracking()
            .Where(p => p.CompanyId == companyId && p.ProductCen == productCen)
            .Select(p => new { p.Id, p.Name, UnitName = p.Unit!.Name })
            .FirstOrDefaultAsync();

        if (product == null) return (0, string.Empty, string.Empty);
        return (product.Id, product.Name, product.UnitName);
    }

    public async Task<IEnumerable<Product>> LookupProductsAsync(int companyId, List<string>? productCens, List<string>? skus)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Where(p => p.CompanyId == companyId)
            .AsQueryable();

        if (productCens != null && productCens.Any())
        {
            query = query.Where(p => productCens.Contains(p.ProductCen));
        }

        if (skus != null && skus.Any())
        {
            query = query.Where(p => skus.Contains(p.Sku));
        }

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByQueryAsync(int companyId, string? search, string? categoryCen, string? status)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Unit)
            .Where(p => p.CompanyId == companyId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(p => p.Name.Contains(search) || p.Sku.Contains(search));
        }

        if (!string.IsNullOrEmpty(categoryCen))
        {
            query = query.Where(p => p.Category!.CategoryCen == categoryCen);
        }

        if (!string.IsNullOrEmpty(status))
        {
            bool isActive = status.Equals("ACTIVE", StringComparison.OrdinalIgnoreCase);
            query = query.Where(p => p.IsActive == isActive);
        }

        return await query.ToListAsync();
    }
    
    public async Task<Product?> GetByCenAsync(int companyId, string productCen)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.ProductCen == productCen);
    }
}