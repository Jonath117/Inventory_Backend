using Shared.Domain;

namespace Inventory.Domain.Entities;

public class Unit
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string UnitCen { get; private set; } = null!;
    
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    
    public string? Abbreviation { get; private set; } 
    public bool IsActive { get; private set; }
    
    public Company? Company { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();

    private Unit() { }

    public Unit(int companyId, string name, string? abbreviation)
    {
        CompanyId = companyId;
        SetName(name);
        Abbreviation = abbreviation?.Trim();
        
        
        UnitCen = $"UNI-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la unidad es obligatorio");
        Name = name.Trim();
    }
    
    public void Update(string name, string? abbreviation)
    {
        SetName(name);
        Abbreviation = abbreviation?.Trim();
    }
    
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}