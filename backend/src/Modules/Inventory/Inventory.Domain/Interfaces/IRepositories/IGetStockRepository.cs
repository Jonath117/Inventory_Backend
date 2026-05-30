using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IGetStockRepository
{
    Task<IEnumerable<InventoryStock>> GetCurrentStockAsync(int companyId, string? productCen, string? warehouseCen);
}