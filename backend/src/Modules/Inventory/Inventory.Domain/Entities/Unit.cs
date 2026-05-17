using Shared.Domain;

namespace Inventory.Domain.Entities;

public class Unit
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    
    public Company? Company { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();

    private Unit() { }

    public Unit(int companyId, string name, string? description)
    {
        CompanyId = companyId;
        SetName(name);
        Description = description?.Trim();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la unidad es obligatorio");
        Name = name.Trim();
    }
}