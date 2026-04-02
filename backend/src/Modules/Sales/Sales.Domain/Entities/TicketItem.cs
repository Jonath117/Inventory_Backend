namespace Sales.Domain.Entities;

public class TicketItem
{
    public int Id { get; private set; }
    public int TicketId { get; private set; }
    public int ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string? Note { get; private set; }

    public decimal SubTotal => Quantity * UnitPrice;
    
    private TicketItem() { }

    internal TicketItem(int ticketId, int productId, string productName, decimal quantity, decimal unitPrice,
        string? note)
    {
        TicketId = ticketId;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Note = note;
    }

    internal void UpdateQuantity(decimal newQuantity)
    {
        if (newQuantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero");
        Quantity = newQuantity;
    }
}