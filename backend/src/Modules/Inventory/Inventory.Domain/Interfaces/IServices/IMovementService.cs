using Inventory.Domain.DTOs;

namespace Inventory.Domain.Interfaces.IServices;

public interface IMovementService
{
    Task RegisterMovement(int companyId, MovementDto request);
    Task<StockConsumeContractResponse> ConsumeStockAsync(int companyId, StockConsumeContractRequest request);
    Task<string> IncreaseStockAsync(int companyId, StockIncreaseContractRequest request);
    Task<InventoryDocumentContractDto> CreateDocumentAsync(int companyId, InventoryDocumentContractRequest request);
    Task<IEnumerable<InventoryDocumentContractDto>> GetDocumentsAsync(int companyId, string? documentType, DateTime? from, DateTime? to);
    Task<StockValidationContractResponse> ValidateStockAsync(int companyId, StockValidationContractRequest request);
}