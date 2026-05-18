namespace Inventory.Domain.Interfaces.IRepositories;

public interface IWarehouseRepository
{
    Task<(int Id, string Name)> GetInfoByCenAsync(int companyId, string warehouseCen);
}