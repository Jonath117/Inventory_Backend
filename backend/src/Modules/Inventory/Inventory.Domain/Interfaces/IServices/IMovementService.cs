using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IMovementService
{
    Task RegisterMovement(int companyId, MovementDto request);
    Task<StockConsumeContractResponse> ConsumeStockAsync(int companyId, StockConsumeContractRequest request);
    Task<string> IncreaseStockAsync(int companyId, StockIncreaseContractRequest request);
}