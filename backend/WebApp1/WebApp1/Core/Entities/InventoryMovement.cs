namespace WebApp1.Core.Entities;

public class InventoryMovement
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    
    public string MovementType { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal PreviousStock { get; set; }
    public decimal NewStock { get; set; }
    
    public string? Reason { get; set; }
    public string? Reference { get; set; }
    public string? UserReference { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Company? Company { get; set; }
    public Warehouse? Warehouse { get; set; }
    public Product? Product { get; set; }
    
}