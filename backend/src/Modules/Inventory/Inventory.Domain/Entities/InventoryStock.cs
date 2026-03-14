namespace Inventory.Domain.Entities;

public class InventoryStock
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int WarehouseId { get; set; }
    public int ProductId { get; set; }
    public decimal CurrentStock { get; set; }
    public DateTime LastUpdated { get; set; } =  DateTime.UtcNow;
    
    public Company? Company { get; set; }
    public Warehouse? Warehouse { get; set; }
    public Product? Product { get; set; }
}