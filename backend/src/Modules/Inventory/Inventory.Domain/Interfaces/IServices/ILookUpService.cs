using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface ILookUpService
{
    Task<IEnumerable<ProductLookUpDto>> GetProductsForDropdownAsync(int companyId);
    
    Task<IEnumerable<WarehouseLookUpDto>> GetWarehouseForDropdownAsync(int companyId);
}