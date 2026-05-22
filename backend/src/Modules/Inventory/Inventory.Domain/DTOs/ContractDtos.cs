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

public record StockItemContractDto(
    string ProductCen,
    string Sku,
    string ProductName,
    string WarehouseCen,
    string WarehouseName,
    double Quantity,
    string UnitName,
    DateTime LastUpdated
);

public record InventoryDocumentLineContractRequest(string ProductCen, decimal Quantity);
public record InventoryDocumentContractRequest(string WarehouseCen, string DocumentType, string Source, string ReferenceCen, string? Reason, List<InventoryDocumentLineContractRequest> Items);

public record InventoryDocumentContractDto(
    string DocumentCen,
    string DocumentType,
    DateTime Date,
    string WarehouseCen,
    string WarehouseName,
    string Source,
    string ReferenceCen,
    string? Reason,
    List<GeneratedMovementContractDto> Items
);

public record StockValidationContractRequest(string WarehouseCen, List<StockValidationItemContractDto> Items);

// --- RESPONSES ---
public record StockValidationContractResponse(bool AllAvailable, List<StockRequirementContractDto> Requirements);
public record StockConsumeContractResponse(bool Success, string? DocumentCen, string? DocumentType, List<string> GeneratedMovementCens, List<StockRequirementContractDto> Requirements);
public record InventoryAdjustmentContractResponse(string AdjustmentCen, string Status, List<GeneratedMovementContractDto> GeneratedMovements);