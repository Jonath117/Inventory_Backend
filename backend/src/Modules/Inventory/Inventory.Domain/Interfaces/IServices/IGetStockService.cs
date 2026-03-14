using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IGetStockService
{
    Task<List<StockDto>> GetCurrentStockAsync(int companyId, int? warehouseId = null);
}