using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IWarehouseService
{
    Task<IEnumerable<WarehouseContractDto>> GetWarehousesAsync(int companyId);
}