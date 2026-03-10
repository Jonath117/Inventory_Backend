namespace WebApp1.Domain.DTOs;

public class StockDto
{
    public int ProductId { get; set; }
    public string Sku { get; set; }
    public string ProductName { get; set; }
    public string WarehouseName { get; set; }
    public decimal CurrentStock { get; set; }
    public string UnitOfMeasure { get; set; } = string.Empty;
    public int MinStockAlert { get; set; }
    public DateTime LastUpdated { get; set; }
}