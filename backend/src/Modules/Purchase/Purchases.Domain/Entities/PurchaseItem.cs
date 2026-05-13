namespace Purchases.Domain.Entities;

public class PurchaseItem
{
    public int Id { get; private set; }
    public int PurchaseId { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public Purchase Purchase { get; private set; }
    
    private PurchaseItem() { }

    internal PurchaseItem(int productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}