namespace Sales.Application.Interfaces.ExternalServices;

// venta a inven
public record StockConsumeItemDto(string ProductCen, decimal Quantity);

public record StockConsumeRequestDto(
    string WarehouseCen, 
    string Source, 
    string ReferenceCen, 
    string? Reason, 
    List<StockConsumeItemDto> Items
);

// inventario responde a venta
public record StockRequirementDto(
    string ProductCen, 
    string ProductName, 
    string WarehouseCen, 
    decimal RequestedQuantity, 
    decimal AvailableQuantity, 
    decimal MissingQuantity, 
    string UnitName, 
    string Reason
);

public record StockConsumeResponseDto(
    bool Success, 
    string? DocumentCen, 
    string? DocumentType, 
    List<string> GeneratedMovementCens, 
    List<StockRequirementDto> Requirements
);


public record ProductDetailsDto(
    string ProductCen,
    string Name,
    decimal SalePrice,
    bool IsAvailable
);

public record SellableProductContractDto(
    string ProductCen,
    string Sku,
    string Name,
    string? Description,
    string CategoryName,
    string UnitName,
    double SalePrice,
    double AvailableStock,
    bool IsAvailable
);