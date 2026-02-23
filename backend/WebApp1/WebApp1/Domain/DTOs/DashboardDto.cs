namespace WebApp1.Domain.DTOs;

public class DashboardDto
{
    public int TotalProducts { get; set; }
    public int TotalWarehouses { get; set; }
    public decimal TotalStockQuantity { get; set; }
    public int LowStockAlerts  {get; set;}
}