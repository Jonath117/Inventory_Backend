using Inventory.Domain.DTOs;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces.IRepositories;
using Inventory.Domain.Interfaces.IServices;

namespace Inventory.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    
    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }


    public async Task<IEnumerable<ProductDto>> GetProductsAsync(int companyId)
    {
        var products =  await _repository.GetProductsAsync(companyId);

        if (products == null || !products.Any())
        {
            return new List<ProductDto>();
        }
        
        return products.Select(p => new ProductDto(
            p.Id,
            p.Sku,
            p.Name,
            p.Description,
            p.Price,
            p.SalePrice,
            p.MinStockAlert,
            p.IsActive,
            p.IsSoldOut,
            p.CategoryId,
            p.Category?.Name ?? "Sin Categoría",
            p.UnitId,
            p.Unit?.Name ?? "Sin Unidad"
            )).ToList();
    }

    public async Task<ProductLookUpDto> CreateProductAsync(ProductCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Sku)) throw new ArgumentException("El codigo sku es obligatorio");
        if(string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("El nombre es obligatorio");
        
        bool skuExists = await _repository.ExistsBySkuAsync(dto.CompanyId, dto.Sku);

        if (skuExists)
            throw new ArgumentException($"Ya existe un producto con el codigo SKU: {dto.Sku}");

        var newProduct = new Product
        {
            CompanyId = dto.CompanyId,
            Sku = dto.Sku.Trim().ToUpper(),
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim(),
            Price = dto.Price,
            SalePrice = dto.SalePrice,
            MinStockAlert = dto.MinStockAlert,
            CategoryId = dto.CategoryId,
            UnitId = dto.UnitId,
            IsActive = dto.IsActive,
            IsSoldOut = true
        };
        
        var savedProduct = await _repository.AddAsync(newProduct);
        
        return new ProductLookUpDto(savedProduct.Id, savedProduct.Sku, savedProduct.Name);
    }

    public async Task<ProductLookUpDto> EditProductAsync(int productId, ProductUpdateDto dto)
    {
        var product = await _repository.GetByIdAsync(dto.CompanyId, productId);
        if (product == null)
            throw new KeyNotFoundException("El producto no existe");
        
        bool skuExists = await _repository.ExistsBySkuAsync(dto.CompanyId, dto.Sku, productId);
        if (skuExists)
            throw new InvalidOperationException($"El codigo SKU {dto.Sku} ya esta en uso por otro producto");
        
        product.Sku = dto.Sku.Trim().ToUpper();
        product.Name = dto.Name.Trim();
        product.Description = dto.Description?.Trim();
        product.Price = dto.Price;
        product.SalePrice = dto.SalePrice;
        product.MinStockAlert = dto.MinStockAlert;
        product.CategoryId = dto.CategoryId;
        product.IsActive = dto.IsActive;
        product.UnitId = dto.UnitId;
        
        await _repository.UpdateAsync(product);

        return new ProductLookUpDto(productId, product.Sku, product.Name);
    }

    public async Task DesactiveProductAsync(int companyId, int productId)
    {
        var product = await _repository.GetByIdAsync(companyId, productId);
        if (product == null)
            throw new KeyNotFoundException("El producto no existe");

        product.IsActive = false;
        
        await _repository.UpdateAsync(product);
    }

    public async Task ActivateProductAsync(int companyId, int productId)
    {
        var product = await _repository.GetByIdAsync(companyId, productId);
        if (product == null)
            throw new KeyNotFoundException("El producto no existe");

        product.IsActive = true;
        
        await _repository.UpdateAsync(product);
    }
}