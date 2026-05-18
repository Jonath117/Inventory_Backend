namespace Purchases.Domain.Entities;

public class Supplier
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string SupplierCen { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    private Supplier() { }

    public Supplier(int companyId, string name)
    {
        CompanyId = companyId;
        
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del proveedor es obligatorio");
            
        Name = name.Trim();
        SupplierCen = $"SUP-{Guid.NewGuid():N}";
    }
}