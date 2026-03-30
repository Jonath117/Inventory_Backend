namespace Inventory.Domain.DTOs;

public record ProductLookUpDto(int Id, string Sku, string Name);
public record WarehouseLookUpDto(int Id, string Name);

public record StockProductDto(
    int ProductId, 
    string Sku, 
    string ProductName, 
    string WarehouseName, 
    decimal CurrentStock, 
    string UnitOfMeasure, 
    int MinStockAlert, 
    DateTime LastUpdated
);

public record ProductDto(
    int Id, 
    string Sku, 
    string Name, 
    string? Description, 
    decimal Price,       
    decimal SalePrice,   
    int MinStockAlert, 
    bool IsActive, 
    bool IsSoldOut, 
    int CategoryId,
    string CategoryName,
    int UnitId,
    string UnitName      
);

public record ProductCreateDto(
    string Sku, 
    string Name, 
    string? Description, 
    decimal Price, 
    decimal SalePrice, 
    int MinStockAlert, 
    int CategoryId, 
    int UnitId,
    bool IsActive = true,
    int CompanyId = 0
);

public record ProductUpdateDto(
    string Sku, 
    string Name, 
    string? Description, 
    decimal Price, 
    decimal SalePrice, 
    int MinStockAlert, 
    int CategoryId, 
    int UnitId,
    bool IsActive,
    int CompanyId = 0 
);