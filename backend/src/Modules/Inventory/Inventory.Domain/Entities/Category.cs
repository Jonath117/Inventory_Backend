using Shared.Domain;

namespace Inventory.Domain.Entities;

public class Category
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string CategoryCen { get; private set; } = null!;
    
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    
    public Company? Company { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();

    private Category() { }

    public Category(int companyId, string name, string? description)
    {
        CompanyId = companyId;
        SetName(name);
        Description = description?.Trim();
        IsActive = true;
        
        CategoryCen = $"CAT-{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
    }

    public void Update(string name, string? description)
    {
        SetName(name);
        Description = description?.Trim();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la categoria es obligatorio");
        Name = name.Trim();
    }
}