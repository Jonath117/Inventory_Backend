using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface ILookUpRepository
{
    Task<IEnumerable<Product>> GetProductsForDropdownAsync(int companyId);
    
    Task<IEnumerable<Warehouse>> GetWarehouseForDropdownAsync(int companyId);
}