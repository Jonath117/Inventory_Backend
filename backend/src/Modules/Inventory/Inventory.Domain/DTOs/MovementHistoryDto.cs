namespace Inventory.Domain.DTOs;

public class MovementHistoryDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string MovementType { get; set; }
    public decimal Quantity { get; set; }
    public decimal PreviousStock { get; set; }
    public decimal NewStock { get; set; }
    public string? Reason { get; set; }
    public string? Reference { get; set; }
    public string WareHouseName { get; set; } = string.Empty;
}