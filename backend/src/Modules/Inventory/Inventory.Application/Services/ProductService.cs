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
            p.ProductCen,
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
        bool skuExists = await _repository.ExistsBySkuAsync(dto.CompanyId, dto.Sku);
        if (skuExists)
            throw new ArgumentException($"Ya existe un producto con el codigo SKU: {dto.Sku}");
        
        var newProduct = new Product(dto.CompanyId, dto.Sku, dto.Name, dto.CategoryId, dto.UnitId,
            dto.Price, dto.SalePrice, dto.MinStockAlert, dto.Description);
    
        var savedProduct = await _repository.AddAsync(newProduct);
        return new ProductLookUpDto(savedProduct.ProductCen, savedProduct.Sku, savedProduct.Name);
    }

    public async Task<ProductLookUpDto> EditProductAsync(int productId, ProductUpdateDto dto)
    {
        var product = await _repository.GetByIdAsync(dto.CompanyId, productId);
        if (product == null)
            throw new KeyNotFoundException("El producto no existe");
        
        bool skuExists = await _repository.ExistsBySkuAsync(dto.CompanyId, dto.Sku, productId);
        if (skuExists)
            throw new InvalidOperationException($"El codigo SKU {dto.Sku} ya esta en uso por otro producto");
        
        product.Update(dto.Sku, dto.Name, dto.Description, dto.Price, dto.SalePrice,
            dto.MinStockAlert, dto.CategoryId, dto.UnitId, dto.IsActive);
        
        await _repository.UpdateAsync(product);

        return new ProductLookUpDto(product.ProductCen, product.Sku, product.Name);
    }

    public async Task DesactiveProductAsync(int companyId, int productId)
    {
        var product = await _repository.GetByIdAsync(companyId, productId);
        if (product == null)
            throw new KeyNotFoundException("El producto no existe");

        product.Deactivate();
        
        await _repository.UpdateAsync(product);
    }

    public async Task ActivateProductAsync(int companyId, int productId)
    {
        var product = await _repository.GetByIdAsync(companyId, productId);
        if (product == null)
            throw new KeyNotFoundException("El producto no existe");

        product.Activate();
        
        await _repository.UpdateAsync(product);
    }
}