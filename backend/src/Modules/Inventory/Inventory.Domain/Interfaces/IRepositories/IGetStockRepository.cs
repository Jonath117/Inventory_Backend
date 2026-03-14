using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IGetStockRepository
{
    Task<List<InventoryStock>> GetStockAsync(int companyId, int? warehouseId);
}