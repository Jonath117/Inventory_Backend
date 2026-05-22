using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IInventoryService
{
    Task<InventoryDashboardContractDto> GetDashboardMetricsAsync(int companyId);
}