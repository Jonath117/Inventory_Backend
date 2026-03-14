using Inventory.Domain.DTOs;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repository;

    public InventoryService(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardDto> GetDashboardMetricsAsync(int companyId)
    {
        var companyExists = await _repository.CompanyExistsAsync(companyId);

        if (!companyExists)
        {
            throw new Exception("La compañia no existe");
        }
        
        var totalProducts = await _repository.GetTotalProductsAsync(companyId);
        var totalWarehouses = await _repository.GetTotalWarehousesAsync(companyId);
        var totalStock = await _repository.GetTotalStockAsync(companyId);
        var lowStockCount = await _repository.GetLowStockAlertsAsync(companyId);
        
        return new DashboardDto()
        {
            TotalProducts = totalProducts,
            TotalStockQuantity = totalStock,
            TotalWarehouses = totalWarehouses,
            LowStockAlerts = lowStockCount
        };
    }
}