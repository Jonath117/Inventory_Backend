using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IGetProductKardexService
{
    Task<List<MovementHistoryDto>> GetProductKardexAsync(int companyId, int productId, int? warehouseId = null);
}