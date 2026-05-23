namespace Sales.Domain.Entities;

public class TicketItem
{
    public int Id { get; private set; }
    public int TicketId { get; private set; }
    public string TicketItemCen { get; private set; } = null!;
    public string ProductCen { get; private set; } = null!;
    
    
    public string ProductName { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public string? Note { get; private set; }

    public decimal SubTotal => Quantity * UnitPrice;
    
    private TicketItem() { }

    internal TicketItem(int ticketId, string ticketItemCen, string productCen, string productName, decimal quantity, decimal unitPrice, string? note)
    {
        TicketId = ticketId;
        TicketItemCen = ticketItemCen; 
        ProductCen = productCen;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Note = note;
    }

    public void UpdateQuantity(decimal newQuantity)
    {
        if (newQuantity <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero");
        Quantity = newQuantity;
    }
}