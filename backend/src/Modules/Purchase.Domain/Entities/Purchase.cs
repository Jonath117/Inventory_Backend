using Purchase.Domain.Enums;

namespace Purchase.Domain.Entities;

public class Purchase
{
    int Id { get;}
    public string Supplier { get; private set; }
    public DateTime Date { get; private set; }
    public StatusPurchase Status { get; private set; }
    
    private Purchase() {}

    internal Purchase(string supplier, DateTime date, StatusPurchase status)
    {
        Supplier = supplier;
        Date = DateTime.UtcNow;
        Status = StatusPurchase.Pending;
    }
    
    
}