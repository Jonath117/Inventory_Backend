using Purchases.Domain.Enums;

namespace Purchases.Domain.Entities;

public class Purchase
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string Supplier { get; private set; }
    public DateTime Date { get; private set; }
    public StatusPurchase Status { get; private set; }

    public List<PurchaseItem> Items { get; private set; } = new();
    
    private Purchase() {}

    internal Purchase(int companyId, string supplier)
    {
        CompanyId = companyId;
        Supplier = supplier;
        Date = DateTime.UtcNow;
        Status = StatusPurchase.Pending;
    }

    public void AddItem(int productId, int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero");
        Items.Add(new PurchaseItem(productId, quantity));
    }

    public void RemoveItem(int productId, int quantity)
    {
        if(quantity <= 0) throw  new ArgumentException("La cantidad debe ser mayor a cero");
        Items.RemoveAt(Items.Count - 1);
    }

    public void Confirm()
    {
        if (Status != StatusPurchase.Pending)
            throw new InvalidOperationException("Solo se puede confirmar compras con estado pendiente");
        Status = StatusPurchase.Confirmed;
    }

    public void Cancel()
    {
        if (Status == StatusPurchase.Confirmed)
            throw new InvalidOperationException("No se pueden cancelar compras confirmadas");
        Status = StatusPurchase.Cancelled;
    }

}