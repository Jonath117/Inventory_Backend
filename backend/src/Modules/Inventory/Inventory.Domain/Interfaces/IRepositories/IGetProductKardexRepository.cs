using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IGetProductKardexRepository
{
    Task<List<InventoryMovement>> GetMovementsAsync(int companyId, int productId, int? warehouseId);
}