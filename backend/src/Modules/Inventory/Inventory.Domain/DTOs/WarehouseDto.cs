namespace Inventory.Domain.DTOs;

public record WarehouseContractDto(
    string WarehouseCen,
    string Name,
    string? Description,
    bool IsActive
);