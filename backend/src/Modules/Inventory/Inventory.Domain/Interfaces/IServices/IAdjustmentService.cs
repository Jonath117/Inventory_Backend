using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IAdjustmentService
{
    Task<InventoryAdjustmentContractResponse> RegisterAdjustmentAsync(int companyId, InventoryAdjustmentContractRequest request);
}