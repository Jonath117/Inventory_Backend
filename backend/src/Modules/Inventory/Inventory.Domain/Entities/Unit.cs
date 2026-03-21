using Shared.Domain;

namespace Inventory.Domain.Entities;

public class Unit
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public Company? Company { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}