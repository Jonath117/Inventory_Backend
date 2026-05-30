using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface ILookUpService
{
    Task<IEnumerable<ProductLookUpDto>> GetProductsForDropdown(int companyId);
    
    Task<IEnumerable<WarehouseLookUpDto>> GetWarehouseForDropdown(int companyId);
}