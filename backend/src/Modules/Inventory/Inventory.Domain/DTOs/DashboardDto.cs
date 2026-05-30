namespace Inventory.Domain.DTOs;

public record InventoryDashboardContractDto(
    int TotalProducts,
    double TotalStockValue,
    int LowStockAlerts,
    int OutOfStockCount
);