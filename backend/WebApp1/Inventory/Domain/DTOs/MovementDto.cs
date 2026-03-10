namespace WebApp1.Domain.DTOs;

public class MovementDto
{
    public int ProductId { get; set; }
    public int WarehouseId { get; set; }

    public string MovementType { get; set; } = string.Empty;
    
    public decimal Quantity { get; set; }
    
    public string? Reference { get; set; }
    public string? Reason { get; set; }
    
}