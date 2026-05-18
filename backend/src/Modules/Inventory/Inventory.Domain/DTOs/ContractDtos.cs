namespace Inventory.Domain.DTOs;

public record StockValidationItemContractDto(string ProductCen, decimal Quantity);
public record StockConsumeItemContractDto(string ProductCen, decimal Quantity);
public record StockRequirementContractDto(string ProductCen, string ProductName, string WarehouseCen, decimal RequestedQuantity, decimal AvailableQuantity, decimal MissingQuantity, string UnitName, string Reason);
public record GeneratedMovementContractDto(string MovementCen, string ProductCen, string WarehouseCen, decimal Quantity, string MovementType);

// --- REQUESTS ---
public record StockConsumeContractRequest(string WarehouseCen, string Source, string ReferenceCen, string? Reason, List<StockConsumeItemContractDto> Items);
public record StockIncreaseContractRequest(string WarehouseCen, string Source, string ReferenceCen, string? Reason, List<StockValidationItemContractDto> Items);
public record InventoryAdjustmentLineContractRequest(string ProductCen, decimal Quantity, string AdjustmentType); // "IN" o "OUT"
public record InventoryAdjustmentContractRequest(string WarehouseCen, string Reason, List<InventoryAdjustmentLineContractRequest> Lines);

// --- RESPONSES ---
public record StockConsumeContractResponse(bool Success, string? DocumentCen, string? DocumentType, List<string> GeneratedMovementCens, List<StockRequirementContractDto> Requirements);
public record InventoryAdjustmentContractResponse(string AdjustmentCen, string Status, List<GeneratedMovementContractDto> GeneratedMovements);