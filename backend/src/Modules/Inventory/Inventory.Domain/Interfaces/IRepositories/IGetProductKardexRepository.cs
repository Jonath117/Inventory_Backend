using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IGetProductKardexRepository
{
    Task<IEnumerable<InventoryMovement>> GetProductKardexAsync(int companyId, string productCen, string? warehouseCen, DateTime? from = null, DateTime? to = null);
}