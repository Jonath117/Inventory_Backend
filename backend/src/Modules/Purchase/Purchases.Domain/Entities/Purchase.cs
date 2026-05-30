using Purchases.Domain.Enums;

namespace Purchases.Domain.Entities;

public class Purchase
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string OrderCen { get; private set; } = null!;
    
    public string SupplierCen { get; private set; } = null!;
    public string WarehouseCen { get; private set; } = null!;
    
    public DateTime CreatedAt { get; private set; } 
    public DateTime? ConfirmedAt { get; private set; }
    public StatusPurchase Status { get; private set; }

    private readonly List<PurchaseItem> _items = new();
    public IReadOnlyCollection<PurchaseItem> Items => _items.AsReadOnly();
    
    private Purchase() {}

    public Purchase(int companyId, string supplierCen, string warehouseCen)
    {
        CompanyId = companyId;
        SupplierCen = supplierCen;
        WarehouseCen = warehouseCen;
        
        CreatedAt = DateTime.UtcNow;
        Status = StatusPurchase.Pending;
        
        string randomSuffix = Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
        OrderCen = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{randomSuffix}";
    }

    public void AddItem(string productCen, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero");
        if (string.IsNullOrWhiteSpace(productCen)) throw new ArgumentException("El producto es obligatorio");
        
        _items.Add(new PurchaseItem(productCen, quantity));
    }

    public void Confirm()
    {
        if (Status != StatusPurchase.Pending)
            throw new InvalidOperationException("Solo se puede confirmar compras con estado pendiente");
            
        Status = StatusPurchase.Confirmed;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == StatusPurchase.Confirmed)
            throw new InvalidOperationException("No se pueden cancelar compras confirmadas");
            
        Status = StatusPurchase.Cancelled;
    }
}