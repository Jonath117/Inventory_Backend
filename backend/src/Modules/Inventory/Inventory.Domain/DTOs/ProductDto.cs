namespace Inventory.Domain.DTOs;
//
public record ProductLookUpDto(string ProductCen, string Sku, string Name);
public record WarehouseLookUpDto(int Id, string Name);
//
public record StockProductDto(
    string ProductCen,     
    string Sku, 
    string ProductName, 
    string WarehouseName, 
    decimal CurrentStock, 
    string UnitOfMeasure, 
    decimal MinStockAlert, 
    DateTime LastUpdated
);
//
// public record ProductDto(
//     int Id, 
//     string ProductCen,
//     string Sku, 
//     string Name, 
//     string? Description, 
//     decimal Price,       
//     decimal SalePrice,   
//     int MinStockAlert, 
//     bool IsActive, 
//     bool IsSoldOut, 
//     int CategoryId,
//     string CategoryName,
//     int UnitId,
//     string UnitName      
// );
//
// public record ProductCreateDto(
//     string Sku, 
//     string Name, 
//     string? Description, 
//     decimal Price, 
//     decimal SalePrice, 
//     int MinStockAlert, 
//     int CategoryId, 
//     int UnitId,
//     bool IsActive = true,
//     int CompanyId = 0
// );
//
// public record ProductUpdateDto(
//     string Sku, 
//     string Name, 
//     string? Description, 
//     decimal Price, 
//     decimal SalePrice, 
//     int MinStockAlert, 
//     int CategoryId, 
//     int UnitId,
//     bool IsActive,
//     int CompanyId = 0 
// );

public record CreateProductContractRequest(
    string Sku, 
    string Name, 
    string? Description, 
    string CategoryCen, 
    string UnitCen, 
    decimal SalePrice, 
    decimal? CostPrice, 
    int ReorderLevel, 
    string? StationCode
);

//PUT
public record UpdateProductContractRequest(
    string Sku, 
    string Name, 
    string? Description, 
    string CategoryCen, 
    string UnitCen, 
    decimal SalePrice, 
    decimal? CostPrice, 
    int ReorderLevel, 
    string? StationCode
);

//PATCH
public record UpdateProductStatusContractRequest(
    string Status, 
    string? Reason
);

//respuesta oficial
public record ProductContractDto(
    string ProductCen, 
    string Sku, 
    string Name, 
    string? Description, 
    string CategoryCen, 
    string CategoryName,
    string UnitCen, 
    string UnitName,
    decimal SalePrice, 
    decimal? CostPrice, 
    decimal ReorderLevel, 
    string Status, 
    string? StationCode
);

public record ProductLookupContractRequest(
    List<string>? ProductCens,
    List<string>? Skus
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