using Inventory.Domain.Entities;
using Shared.Domain;

public class Product
{
    public int Id { get; private set; }
    public int CompanyId { get; private set; }
    public string ProductCen { get; private set; } = null!;
    
    public int CategoryId { get; private set; }
    public int UnitId { get; private set; }
    
    public string Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    
    public int MinStockAlert { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public decimal SalePrice { get; private set; }
    public decimal Price { get; private set; }
    public bool IsSoldOut { get; private set; }
    
    public Company? Company { get; set; }
    public Category? Category { get; set; }
    public Unit? Unit { get; set; }
    
    private Product() { }
    
    public Product(int companyId, string sku, string name, int categoryId, int unitId,
        decimal price, decimal salePrice, int minStockAlert, string? description)
    {
        CompanyId = companyId;
        SetSku(sku);
        SetName(name);
        CategoryId = categoryId;
        UnitId = unitId;
        Price = price;
        SalePrice = salePrice;
        MinStockAlert = minStockAlert > 0 ? minStockAlert : 5;
        Description = description?.Trim();
        
        ProductCen = Guid.NewGuid().ToString("N");
        IsActive = true;
        IsSoldOut = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string sku, string name, string? description, decimal price, decimal salePrice,
        int minStockAlert, int categoryId, int unitId, bool isActive)
    {
        SetSku(sku);
        SetName(name);
        Description = description?.Trim();
        Price = price;
        SalePrice = salePrice;
        MinStockAlert = minStockAlert > 0 ? minStockAlert : 5;
        CategoryId = categoryId;
        UnitId = unitId;
        IsActive = isActive;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void MarkSoldOut(bool isSoldOut)
    {
        IsSoldOut = isSoldOut;
    }

    private void SetSku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("El codigo SKU es obligatorio");
        Sku = sku.Trim().ToUpper();
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es obligatorio");
        Name = name.Trim();
    }
}