using Shared.Domain;

namespace Inventory.Domain.Entities;

public class InventoryStock
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public int WarehouseId { get; private set; }
    public int ProductId { get; private set; }
    public decimal CurrentStock { get; private set; }
    public DateTime LastUpdated { get; private set; }
    
    public Company? Company { get; set; }
    public Warehouse? Warehouse { get; set; }
    public Product? Product { get; set; }

    private InventoryStock() { }

    public InventoryStock(int companyId, int warehouseId, int productId, decimal initialStock)
    {
        if (initialStock < 0)
            throw new ArgumentException("El stock inicial no puede ser negativo");

        CompanyId = companyId;
        WarehouseId = warehouseId;
        ProductId = productId;
        CurrentStock = initialStock;
        LastUpdated = DateTime.UtcNow;
    }

    public void AdjustStock(decimal newStock)
    {
        if (newStock < 0)
            throw new InvalidOperationException("El stock no puede quedar en negativo");

        CurrentStock = newStock;
        LastUpdated = DateTime.UtcNow;
    }
}