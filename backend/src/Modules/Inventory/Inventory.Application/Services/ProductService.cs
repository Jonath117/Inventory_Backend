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
            p.Category?.Name ?? "Sin Categoría", 
            p.Unit?.Name ?? "Sin Unidad"
            )).ToList();
    }
}