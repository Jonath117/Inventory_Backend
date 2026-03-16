using Shared.Domain;

namespace Inventory.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public int CategoryId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string? Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string UnitOfMeasure { get; set; } = "UNIDAD";
    public int MinStockAlert { get; set; } = 5;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public Company? Company { get; set; }
    public Category? Category { get; set; }
}