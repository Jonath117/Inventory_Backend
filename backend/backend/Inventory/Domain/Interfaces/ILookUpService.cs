using WebApp1.Domain.DTOs;

namespace WebApp1.Domain.Interfaces;

public interface ILookUpService
{
    Task<IEnumerable<ProductLookUpDto>> GetProductsForDropdownAsync(int companyId);
    
    Task<IEnumerable<WarehouseLookUpDto>> GetWarehouseForDropdownAsync(int companyId);
}