namespace Purchases.Domain.Entities;

public class PurchaseItem
{
    public int Id { get; private set; }
    public int PurchaseId { get; private set; }
    
    public string ProductCen { get; private set; } = null!; 
    
    public int Quantity { get; private set; }
    public Purchase Purchase { get; private set; } = null!;
    
    private PurchaseItem() { }

    internal PurchaseItem(string productCen, int quantity)
    {
        ProductCen = productCen;
        Quantity = quantity;
    }
}