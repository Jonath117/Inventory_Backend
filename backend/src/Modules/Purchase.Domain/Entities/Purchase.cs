using Purchase.Domain.Enums;

namespace Purchase.Domain.Entities;

public class Purchase
{
    public int Id { get; private set; }
    public string Supplier { get; private set; }
    public DateTime Date { get; private set; }
    public StatusPurchase Status { get; private set; }

    public List<PurchaseItem> Items { get; private set; } = new();
    
    private Purchase() {}

    internal Purchase(string supplier, DateTime date, StatusPurchase status)
    {
        Supplier = supplier;
        Date = DateTime.UtcNow;
        Status = StatusPurchase.Pending;
    }
    
    
}