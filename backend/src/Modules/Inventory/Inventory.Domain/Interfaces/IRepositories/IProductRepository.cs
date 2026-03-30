using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IProductRepository
{
    Task<List<Product>> GetProductsAsync(int companyId);

    Task<bool> ExistsBySkuAsync(int companyId, string sku, int? excludeProductId = null);

    Task<Product> AddAsync(Product product);

    Task<Product?> GetByIdAsync(int companyId, int productId);

    Task UpdateAsync(Product product);
}