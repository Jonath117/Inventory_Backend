namespace Inventory.Domain.DTOs;

public class RestockEvent
{
    public string Producto { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
}
