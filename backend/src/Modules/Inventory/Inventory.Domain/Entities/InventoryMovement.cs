using Shared.Domain;

namespace Inventory.Domain.Entities;

public class InventoryMovement
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public int WarehouseId { get; private set; }
    public int ProductId { get; private set; }
    
    public string MovementType { get; private set; } = null!;
    public decimal Quantity { get; private set; }
    public decimal PreviousStock { get; private set; }
    public decimal NewStock { get; private set; }
    
    public string? Reason { get; private set; }
    public string? Reference { get; private set; }
    public string? UserReference { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Company? Company { get; set; }
    public Warehouse? Warehouse { get; set; }
    public Product? Product { get; set; }

    private InventoryMovement() { }

    public InventoryMovement(int companyId, int warehouseId, int productId,
        string movementType, decimal quantity, decimal previousStock, decimal newStock,
        string? reason, string? reference, string? userReference)
    {
        if (string.IsNullOrWhiteSpace(movementType))
            throw new ArgumentException("El tipo de movimiento es obligatorio");

        CompanyId = companyId;
        WarehouseId = warehouseId;
        ProductId = productId;
        MovementType = movementType;
        Quantity = quantity;
        PreviousStock = previousStock;
        NewStock = newStock;
        Reason = reason;
        Reference = reference;
        UserReference = userReference;
        CreatedAt = DateTime.UtcNow;
    }
}