namespace Inventory.Domain.DTOs;

public class AdjustmentDto
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }
    public decimal Quantity { get; set; }
    public string Reason { get; set; } = string.Empty;
}