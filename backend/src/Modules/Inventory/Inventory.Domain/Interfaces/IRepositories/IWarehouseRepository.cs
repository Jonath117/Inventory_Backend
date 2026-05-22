using Inventory.Domain.Entities;

namespace Inventory.Domain.Interfaces.IRepositories;

public interface IWarehouseRepository
{
    Task<(int Id, string Name)> GetInfoByCenAsync(int companyId, string warehouseCen);
    Task<IEnumerable<Warehouse>> GetAllAsync(int companyId);
    Task<Warehouse?> GetByCenAsync(int companyId, string warehouseCen);
}