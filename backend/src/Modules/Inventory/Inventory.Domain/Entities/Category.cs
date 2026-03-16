using Shared.Domain;

namespace Inventory.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; }
    
    public Company? Company { get; set; }
}