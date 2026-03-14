using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface ILookUpRepository
{
    Task<IEnumerable<ProductLookUpDto>> GetProductsForDropdownAsync(int companyId);
    
    Task<IEnumerable<WarehouseLookUpDto>> GetWarehouseForDropdownAsync(int companyId);
}