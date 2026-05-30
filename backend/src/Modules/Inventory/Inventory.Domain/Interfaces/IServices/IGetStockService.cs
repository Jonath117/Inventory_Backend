using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IGetStockService
{
    Task<IEnumerable<StockItemContractDto>> GetCurrentStockAsync(int companyId, string? productCen, string? warehouseCen);
}