using Shared.Domain;

namespace Inventory.Domain.Entities;

public class Warehouse
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string WarehouseCen { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Address { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Company? Company { get; set; }

    private Warehouse() { }

    public Warehouse(int companyId, string name, string? address)
    {
        CompanyId = companyId;
        SetName(name);
        Address = address;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        
        WarehouseCen = $"WH-{Guid.NewGuid():N}";
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del almacen es obligatorio");
        Name = name.Trim();
    }
}