using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync(int companyId);
    
    Task<Product?> GetByProductCenAsync(int companyId, string productCen);
    
    Task<bool> ExistsBySkuAsync(int companyId, string sku, string? excludeProductCen = null);
    
    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
    
    Task<Product?> GetByIdAsync(int companyId, int productId);
    
    Task<(int Id, string Name, string UnitName)> GetProductInfoByCenAsync(int companyId, string productCen);
    Task<IEnumerable<Product>> LookupProductsAsync(int companyId, List<string>? productCens, List<string>? skus);
    Task<IEnumerable<Product>> GetProductsByQueryAsync(int companyId, string? search, string? categoryCen, string? status);
    
    Task<Product?> GetByCenAsync(int companyId, string productCen);
}