using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IProductRepository
{
    Task<List<Product>> GetProductsAsync(int companyId);
    
    // Task CreateProductAsync(Product product);
    //
    // Task EditProductAsync(Product product);
    // Task DesactiveProductAsync(Product product);
    
}