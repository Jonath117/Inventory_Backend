namespace Inventory.Domain.Interfaces.IRepositories;

public interface IInventoryRepository
{
    Task<bool> CompanyExistsAsync(int companyId);

    Task<int> GetTotalProductsAsync(int companyId);

    Task<int> GetTotalWarehousesAsync(int companyId);

    Task<decimal> GetTotalStockAsync(int companyId);

    Task<int> GetLowStockAlertsAsync(int companyId);
}